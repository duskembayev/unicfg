using unicfg.Base.Analysis;
using unicfg.Base.Extensions;
using unicfg.Evaluation.EmitModel;

namespace unicfg.Evaluation;

public sealed partial class Workspace
{
    public async Task<EmitResult> EmitAsync(
        string outputDirectory,
        DocumentOutput documentOutput,
        CancellationToken cancellationToken)
    {
        var evaluationContext = new EvaluationContext(
            outputDirectory,
            _entries.ToImmutableArrayOfValues(),
            _registry.ToImmutableDictionary());

        return await EmitDocumentAsync(documentOutput, evaluationContext, cancellationToken);
    }

    public async Task<ImmutableArray<EmitResult>> EmitAllAsync(
        string outputDirectory,
        CancellationToken cancellationToken)
    {
        if (_outputs.Count == 0)
        {
            _diagnostics.Report(DiagnosticDescriptor.NothingToEmit);
            return ImmutableArray<EmitResult>.Empty;
        }

        var evaluationContext = new EvaluationContext(
            outputDirectory,
            _entries.ToImmutableArrayOfValues(),
            _registry.ToImmutableDictionary());

        var emitting = _outputs.Select(output => EmitDocumentAsync(output, evaluationContext, cancellationToken));
        var results = await Task.WhenAll(emitting);

        return results.ToImmutableArray();
    }

    private async Task<EmitResult> EmitDocumentAsync(
        DocumentOutput documentOutput,
        EvaluationContext evaluationContext,
        CancellationToken cancellationToken)
    {
        // new Document(evaluationContext.OutputDirectory, )
        throw new NotImplementedException();
    }
}