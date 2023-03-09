using static unicfg.Model.Analysis.DiagnosticDescriptor;

namespace unicfg.Evaluation;

public sealed class EvaluatorImpl
{
    private readonly Diagnostics _diagnostics;
    private readonly IPropertyResolver _propertyResolver;

    public EvaluatorImpl(IPropertyResolver propertyResolver, Diagnostics diagnostics)
    {
        _diagnostics = diagnostics;
        _propertyResolver = propertyResolver;
    }

    public StringRef Evaluate(ElementWithValue node)
    {
        if (node.EvaluationState == EvaluationState.Unevaluated)
            EvaluateValue(node);

        return node.EvaluatedValue;
    }

    private void EvaluateValue(ElementWithValue node)
    {
        var dependencyBuffer = new Dictionary<PropertyRef, StringRef>();
        var evalNodes = new Stack<ElementWithValue>();

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

                if (property.EvaluationState == EvaluationState.Error)
                {
                    Report(refValue, ErrorReference, evalNodes);
                    state |= 8;
                    continue;
                }

                if (property.EvaluationState == EvaluationState.Evaluated)
                {
                    dependencyBuffer[refValue.Property] = property.EvaluatedValue;
                    continue;
                }

                evalNodes.Push(property);
                state |= 1;
            }

            if (state == 1)
                continue;

            do
            {
                if (state > 1)
                {
                    evalNode.SetEvaluationError();
                    break;
                }

                var evaluatedValue = EvaluateValue(evalNode.Value, dependencyBuffer);

                if (evaluatedValue is null)
                {
                    evalNode.SetEvaluationError();
                    break;
                }

                evalNode.SetEvaluatedValue(evaluatedValue.Value);
            } while (false);

            evalNodes.Pop();
        }
    }

    private static StringRef? EvaluateValue(IValue value, IReadOnlyDictionary<PropertyRef, StringRef> dependencyValues)
    {
        var propertyValueEvaluator = new PropertyValueEvaluator(dependencyValues);
        value.Accept(propertyValueEvaluator);

        if (propertyValueEvaluator.HasErrors)
            return null;

        return propertyValueEvaluator.Result;
    }

    private static ImmutableArray<RefValue> CollectRefs(IValue value)
    {
        var propertyRefCollector = new PropertyRefCollector();
        value.Accept(propertyRefCollector);
        return propertyRefCollector.GetResult();
    }

    private void Report(IValue value, DiagnosticDescriptor descriptor, IEnumerable<ElementWithValue> evalNodes)
    {
        var arg = string.Join(" -> ", evalNodes.Select(n => n.ToDisplayName()));
        _diagnostics.Report(descriptor, value.SourceRange, arg);
    }
}