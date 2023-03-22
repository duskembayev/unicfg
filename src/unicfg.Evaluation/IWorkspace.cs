using unicfg.Base.Elements;
using unicfg.Base.Primitives;
using unicfg.Evaluation.EmitModel;

namespace unicfg.Evaluation;

public interface IWorkspace
{
    IReadOnlySet<DocumentOutput> Outputs { get; }

    void OpenFrom(string filePath);
    void Open(Document document);

    void OverrideProperty(SymbolRef property, StringRef value);

    Task<EmitResult> EmitAsync(
        string outputDirectory,
        DocumentOutput documentOutput,
        CancellationToken cancellationToken);

    Task<ImmutableArray<EmitResult>> EmitAllAsync(
        string outputDirectory,
        CancellationToken cancellationToken);

    Task<EvaluateResult> EvaluateAllAsync(CancellationToken cancellationToken);

    Task<EvaluateResult> EvaluateAsync(SymbolRef symbol, CancellationToken cancellationToken);
}