using Enhanced.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using unicfg.Base.Analysis;
using unicfg.Base.Elements;
using unicfg.Evaluation.Extensions;

namespace unicfg.Evaluation;

[ContainerEntry(ServiceLifetime.Transient, typeof(IWorkspace))]
public sealed partial class Workspace : IWorkspace
{
    private readonly Dictionary<DocumentKey, Document> _registry;
    private readonly IDocumentResolver _documentResolver;
    private readonly Diagnostics _diagnostics;
    private readonly SortedList<int, Document> _entries;
    private readonly HashSet<DocumentOutput> _outputs;

    private int _priorityIndex;

    public Workspace(IDocumentResolver documentResolver, Diagnostics diagnostics)
    {
        _documentResolver = documentResolver;
        _diagnostics = diagnostics;
        _entries = new SortedList<int, Document>();
        _registry = new Dictionary<DocumentKey, Document>();
        _outputs = new HashSet<DocumentOutput>();
    }

    public IReadOnlySet<DocumentOutput> Outputs => _outputs;

    public void OpenFrom(string filePath)
    {
        Open(_documentResolver.LoadFromFile(filePath, DocumentFormat.Uni));
    }

    public void Open(Document document)
    {
        if (document.Location is null)
            throw new InvalidOperationException();

        _outputs.UnionWith(document.GetOutputs());
        _registry.Add(DocumentKey.FromLocation(document.Location), document);
        _entries.Add(_priorityIndex++, document);
    }
}