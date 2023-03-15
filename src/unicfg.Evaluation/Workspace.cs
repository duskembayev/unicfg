using unicfg.Base.Analysis;
using unicfg.Base.Elements;

namespace unicfg.Evaluation;

public sealed class Workspace
{
    private readonly Dictionary<DocumentKey, Document> _registry;
    private readonly IDocumentResolver _documentResolver;
    private readonly Diagnostics _diagnostics;
    private readonly List<Document> _entries;
    private readonly List<DocumentOutput> _outputs;

    public Workspace(IDocumentResolver documentResolver, Diagnostics diagnostics)
    {
        _documentResolver = documentResolver;
        _diagnostics = diagnostics;
        _entries = new List<Document>();
        _registry = new Dictionary<DocumentKey, Document>();
        _outputs = new List<DocumentOutput>();
    }

    public IReadOnlyList<Document> EntryDocuments => _entries;
    public IReadOnlyList<DocumentOutput> Outputs => _outputs;

    public void OpenFrom(string filePath)
    {
        Open(_documentResolver.LoadFromFile(filePath, DocumentFormat.Uni));
    }

    public void Open(Document document)
    {
        if (document.Location is null)
            throw new InvalidOperationException();

        _entries.Add(document);
        _registry.Add(DocumentKey.FromLocation(document.Location), document);
    }

    public async Task<EmitResult> EmitAsync(
        string outputDirectory,
        DocumentOutput documentOutput,
        CancellationToken cancellationToken)
    {
        var evaluationContext = new EvaluationContext(
            outputDirectory,
            _entries.ToImmutableArray(),
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
            _entries.ToImmutableArray(),
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
        throw new InvalidOperationException();
    }
}

public record EmitResult();