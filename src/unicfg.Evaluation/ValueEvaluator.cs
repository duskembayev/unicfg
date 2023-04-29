using System.Diagnostics;
using System.Text;
using unicfg.Base.Extensions;
using unicfg.Base.SyntaxTree.Values;
using unicfg.Evaluation.Walkers;

namespace unicfg.Evaluation;

internal sealed class ValueEvaluator : IValueEvaluator
{
    private readonly ILogger<ValueEvaluator> _logger;
    private readonly ImmutableArray<Document> _entries;
    private readonly Dictionary<SymbolRef, EmitValue> _values;
    private readonly IDiagnostics _diagnostics;

    public ValueEvaluator(
        ImmutableArray<Document> entries,
        ImmutableDictionary<SymbolRef, StringRef> overrides,
        IDiagnostics diagnostics,
        ILogger<ValueEvaluator> logger)
    {
        _entries = entries;
        _logger = logger;

        _diagnostics = diagnostics;

        _values = new Dictionary<SymbolRef, EmitValue>();

        foreach (var (key, value) in overrides)
        {
            _values[key] = EmitValue.CreateEvaluatedValue(value);
        }
    }

    public async ValueTask<EmitValue> EvaluateAsync(IElementWithValue element, CancellationToken cancellationToken)
    {
        var symbolRef = SymbolRef.Null;

        if (element is ISymbol symbol)
        {
            symbolRef = symbol.GetSymbolRef();
        }

        return await EvaluateAsync(symbolRef, element.Value, cancellationToken).ConfigureAwait(false);
    }

    private async ValueTask<EmitValue> EvaluateAsync(
        SymbolRef symbolRef,
        IValue value,
        CancellationToken cancellationToken)
    {
        if (!SymbolRef.Null.Equals(symbolRef) && _values.TryGetValue(symbolRef, out var emitValue))
        {
            return emitValue;
        }

        var tale = new Stack<(SymbolRef Path, IValue Value)>(3);

        emitValue = null;
        tale.Push((symbolRef, value));

        while (tale.TryPeek(out var item))
        {
            (emitValue, var unresolvedDependency) = await item.Value
                .BuildValueAsync(_values, cancellationToken)
                .ConfigureAwait(false);

            if (emitValue is not null)
            {
                // Null is a valid value for attributes, but we a not able to store it
                if (item.Path != SymbolRef.Null)
                {
                    if (emitValue.State == EvaluationState.Error)
                    {
                        _diagnostics.Report(ErrorReference, BuildResolvePath(tale, item.Path));
                    }

                    _values[item.Path] = emitValue;
                    _logger.LogDebug("Evaluated value for \"{Path}\": {Value}", item.Path, emitValue.Value);
                }

                tale.Pop();
                continue;
            }

            Debug.Assert(unresolvedDependency != SymbolRef.Null);

            var property = FindProperty(unresolvedDependency);

            if (property is null)
            {
                _diagnostics.Report(UnresolvedReference, BuildResolvePath(tale, unresolvedDependency));
                _values[unresolvedDependency] = EmitValue.Error;
                continue;
            }

            if (HasCircularReference(unresolvedDependency, tale))
            {
                _diagnostics.Report(CircularReference, BuildResolvePath(tale, unresolvedDependency));
                _values[unresolvedDependency] = EmitValue.Error;
                continue;
            }

            tale.Push((unresolvedDependency, property.Value));
        }

        Debug.Assert(emitValue != null);
        return emitValue;
    }

    private static string BuildResolvePath(Stack<(SymbolRef Path, IValue Value)> tale, SymbolRef unresolvedDependency)
    {
        var builder = new StringBuilder();

        foreach (var (path, _) in tale.Reverse())
        {
            builder.Append(path);
            builder.Append(" -> ");
        }

        builder.Append(unresolvedDependency);

        return builder.ToString();
    }

    private static bool HasCircularReference(SymbolRef symbolRef, IEnumerable<(SymbolRef Path, IValue Value)> tale)
    {
        return tale.Any(item => item.Path.Equals(symbolRef));
    }

    private PropertySymbol? FindProperty(SymbolRef path)
    {
        for (var index = 1; index <= _entries.Length; index++)
        {
            var depSymbol = _entries[^index].FindSymbol(path);

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
