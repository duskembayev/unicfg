using unicfg.Base.Primitives;
using unicfg.Base.SyntaxTree;
using unicfg.Evaluation.Outputs;

namespace unicfg.Evaluation.Extensions;

public static class DocumentExtensions
{
    public static async ValueTask<ImmutableHashSet<DocumentOutput>> GetOutputsAsync(
        this Document @this,
        CancellationToken cancellationToken)
    {
        var outputCollector = new OutputCollector(cancellationToken);
        await @this.Accept(outputCollector).ConfigureAwait(false);
        return outputCollector.GetResult();
    }

    public static ISymbol? FindSymbol(this Document @this, SymbolRef symbolRef)
    {
        if (symbolRef.Path.Length == 0)
            return @this.RootScope;

        ISymbol? result = @this.RootScope;
        var depth = 0;

        do
        {
            result = ((ScopeSymbol) result).Children.GetValueOrDefault(symbolRef.Path[depth]);
        } while (++depth < symbolRef.Path.Length && result is ScopeSymbol);

        return depth == symbolRef.Path.Length ? result : null;
    }
}