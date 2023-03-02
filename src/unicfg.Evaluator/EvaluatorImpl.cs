using System.Collections.Immutable;
using unicfg.Model.Analysis;
using unicfg.Model.Extensions;
using unicfg.Model.Primitives;
using unicfg.Model.Semantic;
using static unicfg.Model.Analysis.DiagnosticDescriptor;

namespace unicfg.Evaluator;

public sealed class EvaluatorImpl
{
    private readonly Diagnostics _diagnostics;
    private readonly IPropertyResolver _propertyResolver;

    public EvaluatorImpl(IPropertyResolver propertyResolver, Diagnostics diagnostics)
    {
        _diagnostics = diagnostics;
        _propertyResolver = propertyResolver;
    }

    public string Evaluate(SemanticNodeWithValue node)
    {
        if (node.EvaluationState == PropertyEvaluationState.Unevaluated)
            EvaluateValue(node);

        return node.EvaluatedValue;
    }

    private void EvaluateValue(SemanticNodeWithValue node)
    {
        var dependencyBuffer = new Dictionary<PropertyRef, string>();
        var evalNodes = new Stack<SemanticNodeWithValue>();

        evalNodes.Push(node);

        while (evalNodes.TryPeek(out var evalNode))
        {
            dependencyBuffer.Clear();

            var refs = CollectRefs(evalNode.Value);
            var state = 0u;

            foreach (var refValue in refs)
            {
                var property = _propertyResolver.ResolveProperty(refValue.Property);

                if (property is null)
                {
                    Report(refValue, UnresolvedReference, evalNodes);
                    state |= 2;
                    continue;
                }

                if (evalNodes.Contains(property))
                {
                    Report(refValue, CircularReference, evalNodes);
                    state |= 4;
                    continue;
                }

                if (property.EvaluationState == PropertyEvaluationState.Error)
                {
                    Report(refValue, ErrorReference, evalNodes);
                    state |= 8;
                    continue;
                }

                if (property.EvaluationState == PropertyEvaluationState.Evaluated)
                {
                    dependencyBuffer[refValue.Property] = property.EvaluatedValue;
                    continue;
                }

                evalNodes.Push(property);
                state |= 1;
            }

            if (state == 1)
                continue;

            if (state > 1)
                evalNode.SetEvaluationError();
            else
                evalNode.SetEvaluatedValue(EvaluateValue(evalNode.Value, dependencyBuffer));

            evalNodes.Pop();
        }
    }

    private static string EvaluateValue(IValue value, IReadOnlyDictionary<PropertyRef, string> dependencyValues)
    {
        var propertyValueEvaluator = new PropertyValueEvaluator(dependencyValues);
        value.Accept(propertyValueEvaluator);
        return propertyValueEvaluator.GetResult();
    }

    private static ImmutableArray<RefValue> CollectRefs(IValue value)
    {
        var propertyRefCollector = new PropertyRefCollector();
        value.Accept(propertyRefCollector);
        return propertyRefCollector.GetResult();
    }

    private void Report(IValue value, DiagnosticDescriptor descriptor, IEnumerable<SemanticNodeWithValue> evalNodes)
    {
        var arg = string.Join(" -> ", evalNodes.Select(n => n.ToDisplayName()));
        _diagnostics.Report(descriptor, value.SourceRange, arg);
    }
}