using System.Collections.Immutable;
using unicfg.Base.Analysis;
using unicfg.Base.Elements;

namespace unicfg;

public sealed class Workspace
{
    private readonly Dictionary<DocumentKey, Document> _registry;
    private readonly IDocumentResolver _documentResolver;
    private readonly Diagnostics _diagnostics;
    private readonly List<Document> _entryDocuments;
    private readonly List<DocumentOutput> _outputs;

    public Workspace(IDocumentResolver documentResolver, Diagnostics diagnostics)
    {
        _documentResolver = documentResolver;
        _diagnostics = diagnostics;
        _entryDocuments = new List<Document>();
        _registry = new Dictionary<DocumentKey, Document>();
        _outputs = new List<DocumentOutput>();
    }

    public IReadOnlyList<Document> EntryDocuments => _entryDocuments;
    public IReadOnlyList<DocumentOutput> Outputs => _outputs;

    public void OpenFrom(string filePath)
    {
        Open(_documentResolver.LoadFromFile(filePath, DocumentFormat.Uni));
    }

    public void Open(Document document)
    {
        if (document.Location is null)
            throw new InvalidOperationException();

        _entryDocuments.Add(document);
        _registry.Add(DocumentKey.FromLocation(document.Location), document);
    }

    public async Task<ImmutableArray<EmitResult>> EmitAsync(string outputDirectory, CancellationToken cancellationToken)
    {
        if (_outputs.Count == 0)
        {
            _diagnostics.Report(DiagnosticDescriptor.NothingToEmit);
            return ImmutableArray<EmitResult>.Empty;
        }

        var emitting = _outputs.Select(output => EmitDocumentAsync(outputDirectory, output, cancellationToken));
        var results = await Task.WhenAll(emitting);

        return results.ToImmutableArray();
    }

    private async Task<EmitResult> EmitDocumentAsync(
        string outputDirectory,
        DocumentOutput documentOutput,
        CancellationToken cancellationToken)
    {
        
    }
}

public record EmitResult();