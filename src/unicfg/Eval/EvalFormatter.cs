using unicfg.Base.Primitives;
using unicfg.Base.SemanticTree;
using unicfg.Uni.Formatter;

namespace unicfg.Eval;

internal sealed class EvalFormatter : IFormatter
{
    private readonly string _outputPath;
    private readonly TextWriter _writer;

    public EvalFormatter(string outputPath, TextWriter writer)
    {
        _outputPath = outputPath;
        _writer = writer;
    }

    public bool Matches(IReadOnlyDictionary<StringRef, EmitValue> attributes)
    {
        return true;
    }

    public async Task<EmitResult> FormatAsync(SymbolRef scopeRef, EmitScope scope, CancellationToken cancellationToken)
    {
        var (buffer, totalCount, errorCount) = await FormatCoreAsync(scope, cancellationToken).ConfigureAwait(false);
        await _writer.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);

        return new EmitResult(scopeRef, _outputPath, totalCount, errorCount);
    }

    private static async Task<(StringBuilder Buffer, int TotalCount, int ErrorCount)> FormatCoreAsync(
        EmitScope scope,
        CancellationToken cancellationToken)
    {
        await using var bufferWriter = new StringWriter();
        var visitor = new UniFormatVisitor(bufferWriter);

        foreach (var (_, childProperty) in scope.Properties)
        {
            await childProperty.AcceptAsync(visitor, cancellationToken).ConfigureAwait(false);
        }

        foreach (var (_, childScope) in scope.Scopes)
        {
            await childScope.AcceptAsync(visitor, cancellationToken).ConfigureAwait(false);
        }

        return (bufferWriter.GetStringBuilder(), visitor.TotalPropertyCount,
            visitor.TotalPropertyCount - visitor.SuccessPropertyCount);
    }
}
