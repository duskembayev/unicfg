using unicfg.Base.Primitives;
using unicfg.Base.SyntaxTree;

namespace unicfg.Base.SemanticTree;

public sealed class EmitProperty
{
    private readonly Dictionary<StringRef, StringRef> _attributes;

    public EmitProperty()
    {
        _attributes = new Dictionary<StringRef, StringRef>();
    }

    public StringRef Name { get; init; }
    public EmitScope? Parent { get; init; }
    public PropertySymbol? Symbol { get; set; }

    public IReadOnlyDictionary<StringRef, StringRef> Attributes => _attributes;
}