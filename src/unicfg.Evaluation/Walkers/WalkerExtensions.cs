using unicfg.Base.Extensions;
using unicfg.Base.SyntaxTree.Values;

namespace unicfg.Evaluation.Walkers;

internal static class WalkerExtensions
{
    public static async ValueTask<ImmutableHashSet<DocumentOutput>> GetOutputsAsync(
        this Document @this,
        CancellationToken cancellationToken)
    {
        var outputCollector = new DocumentOutputCollector(cancellationToken);
        await @this.Accept(outputCollector).ConfigureAwait(false);
        return outputCollector.GetResult();
    }

    public static async ValueTask<EmitScope> BuildScopeAsync(
        this IValueEvaluator @this,
        SymbolRef scopeRef,
        ImmutableArray<Document> entries,
        ImmutableDictionary<SymbolRef, StringRef> defaults,
        IDiagnostics diagnostics,
        CancellationToken cancellationToken)
    {
        var outputBuilder = new EmitScopeBuilder(@this, defaults, diagnostics, cancellationToken);

        foreach (var entry in entries)
        {
            var symbol = entry.FindSymbol(scopeRef);

            if (symbol is not null)
            {
                await symbol.Accept(outputBuilder).ConfigureAwait(false);
            }
        }

        return outputBuilder.Scope;
    }
    
    public static async ValueTask<(EmitValue? Value, SymbolRef UnresolvedDependency)> BuildValueAsync(
        this IValue @this,
        IReadOnlyDictionary<SymbolRef, EmitValue> dependencies,
        CancellationToken cancellationToken)
    {
        var valueBuilder = new EmitValueBuilder(dependencies, cancellationToken);
        await @this.Accept(valueBuilder).ConfigureAwait(false);
        return (valueBuilder.GetResult(), valueBuilder.UnresolvedDependency);
    }
}
