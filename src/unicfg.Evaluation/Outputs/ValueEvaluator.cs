using System.Diagnostics;
using unicfg.Base.Extensions;
using unicfg.Base.Primitives;
using unicfg.Base.SemanticTree;
using unicfg.Base.SyntaxTree;
using unicfg.Base.SyntaxTree.Values;
using unicfg.Evaluation.Extensions;

namespace unicfg.Evaluation.Outputs;

internal sealed class ValueEvaluator : IValueEvaluator
{
    private readonly EvaluationContext _evaluationContext;
    private readonly Dictionary<SymbolRef, EmitValue> _values;

    public ValueEvaluator(EvaluationContext evaluationContext)
    {
        _evaluationContext = evaluationContext;
        _values = new Dictionary<SymbolRef, EmitValue>();
    }

    public async ValueTask<EmitValue> EvaluateAsync(IElementWithValue element, CancellationToken cancellationToken)
    {
        var symbolRef = SymbolRef.Null;

        if (element is ISymbol symbol)
            symbolRef = symbol.GetSymbolRef();

        return await EvaluateAsync(symbolRef, element.Value, cancellationToken).ConfigureAwait(false);
    }

    private async ValueTask<EmitValue> EvaluateAsync(
        SymbolRef symbolRef,
        IValue value,
        CancellationToken cancellationToken)
    {
        if (!SymbolRef.Null.Equals(symbolRef) && _values.TryGetValue(symbolRef, out var emitValue))
            return emitValue;

        var valueBuilder = new ValueBuilder(_values, cancellationToken);
        var tale = new Stack<(SymbolRef Path, IValue Value)>(3);

        emitValue = null;
        tale.Push((symbolRef, value));

        while (tale.TryPeek(out var item))
        {
            valueBuilder.Reset();
            await item.Value.Accept(valueBuilder).ConfigureAwait(false);
            emitValue = valueBuilder.GetResult();

            if (emitValue is not null)
            {
                if (item.Path != SymbolRef.Null)
                    _values[item.Path] = emitValue;

                tale.Pop();
                continue;
            }

            Debug.Assert(valueBuilder.UnresolvedDependency != SymbolRef.Null);

            var property = FindProperty(valueBuilder.UnresolvedDependency);

            if (property is null)
            {
                _values[valueBuilder.UnresolvedDependency] = EmitValue.Error;
                continue;
            }

            if (HasCircularReference(valueBuilder.UnresolvedDependency, tale))
            {
                _values[valueBuilder.UnresolvedDependency] = EmitValue.Error;
                continue;
            }

            tale.Push((valueBuilder.UnresolvedDependency, property.Value));
        }

        Debug.Assert(emitValue != null);
        return emitValue;
    }

    private static bool HasCircularReference(SymbolRef symbolRef, IEnumerable<(SymbolRef Path, IValue Value)> tale)
    {
        return tale.Any(item => item.Path.Equals(symbolRef));
    }

    private PropertySymbol? FindProperty(SymbolRef path)
    {
        for (var index = 1; index <= _evaluationContext.Entries.Length; index++)
        {
            var depSymbol = _evaluationContext.Entries[^index].FindSymbol(path);

            switch (depSymbol)
            {
                case PropertySymbol propertySymbol:
                    return propertySymbol;
                case ScopeSymbol:
                    return null;
                case null:
                    continue;
                default:
                    throw new ArgumentOutOfRangeException(nameof(depSymbol));
            }
        }

        return null;
    }
}