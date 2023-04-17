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
        var valueStack = new Stack<(SymbolRef Path, IValue Value)>(3);

        valueStack.Push((symbolRef, value));

        while (valueStack.TryPeek(out var item))
        {
            valueBuilder.Reset();
            await item.Value.Accept(valueBuilder).ConfigureAwait(false);
            emitValue = valueBuilder.GetResult();

            if (emitValue is not null)
            {
                _values[item.Path] = emitValue;
                valueStack.Pop();
                continue;
            }

            foreach (var depPath in valueBuilder.UnresolvedDependencies)
            {
                var property = FindProperty(depPath);

                if (property is null)
                {
                    _values[depPath] = EmitValue.Error;
                    continue;
                }

                valueStack.Push((depPath, property.Value));
            }
        }
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