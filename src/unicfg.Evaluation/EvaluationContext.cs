using unicfg.Base.Formatters;
using unicfg.Base.SyntaxTree;

namespace unicfg.Evaluation;

public sealed class EvaluationContext
{
    public EvaluationContext(
        ImmutableArray<Document> entries,
        ImmutableDictionary<DocumentKey, Document> registry,
        ImmutableArray<IFormatter> formatters)
    {
        Entries = entries;
        Registry = registry;
        Formatters = formatters;
    }

    public ImmutableArray<Document> Entries { get; }
    public ImmutableDictionary<DocumentKey, Document> Registry { get; }
    public ImmutableArray<IFormatter> Formatters { get; }
}