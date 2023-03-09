using unicfg.Model.Elements;

namespace unicfg;

public sealed class Workspace
{
    private readonly IDocumentResolver _documentResolver;
    private readonly List<Document> _documents;
    private readonly List<DocumentOutput> _outputs;

    public Workspace(IDocumentResolver documentResolver)
    {
        _documentResolver = documentResolver;
        _documents = new List<Document>();
    }

    public IReadOnlyList<Document> Documents => _documents;
    public IReadOnlyList<DocumentOutput> Outputs => _outputs;

    public void AddFrom(string filePath)
    {
        Add(_documentResolver.LoadFromFile(filePath, DocumentFormat.Uni));
    }

    public void Add(Document document)
    {
        _documents.Add(document);
    }
    
    
}

public interface IDocumentResolver
{
    Document LoadFromFile(string filePath);
    Document LoadFromFile(string filePath, DocumentFormat format);
    Document LoadFromEnvironment();
    Document LoadFromArgs();
}