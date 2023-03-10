using unicfg.Base.Elements;

namespace unicfg.Evaluation;

public sealed class EvaluationContext
{
    private readonly ImmutableArray<Document> _entries;
    private readonly ImmutableDictionary<DocumentKey, Document> _registry;

    public EvaluationContext(
        string outputDirectory,
        ImmutableArray<Document> entries,
        ImmutableDictionary<DocumentKey, Document> registry)
    {
        OutputDirectory = outputDirectory;
        Entries = entries;
        Registry = registry;
    }

    public string OutputDirectory { get; }
    public ImmutableArray<Document> Entries { get; }
    public ImmutableDictionary<DocumentKey, Document> Registry { get; }
}