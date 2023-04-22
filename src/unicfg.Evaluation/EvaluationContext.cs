using unicfg.Base.Formatters;
using unicfg.Base.Primitives;
using unicfg.Base.SyntaxTree;

namespace unicfg.Evaluation;

public sealed class EvaluationContext
{
    public EvaluationContext(ImmutableArray<Document> entries,
        ImmutableDictionary<DocumentKey, Document> registry,
        ImmutableDictionary<SymbolRef, StringRef> defaults,
        ImmutableDictionary<SymbolRef, StringRef> overrides,
        ImmutableArray<IFormatter> formatters)
    {
        Entries = entries;
        Registry = registry;
        Defaults = defaults;
        Formatters = formatters;
        Overrides = overrides;
    }

    public ImmutableArray<Document> Entries { get; }
    public ImmutableDictionary<DocumentKey, Document> Registry { get; }
    public ImmutableDictionary<SymbolRef, StringRef> Defaults { get; }
    public ImmutableDictionary<SymbolRef, StringRef> Overrides { get; }
    public ImmutableArray<IFormatter> Formatters { get; }
}
