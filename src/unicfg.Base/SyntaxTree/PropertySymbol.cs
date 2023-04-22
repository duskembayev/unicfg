using unicfg.Base.Primitives;
using unicfg.Base.SyntaxTree.Values;

namespace unicfg.Base.SyntaxTree;

public sealed class PropertySymbol : ISymbol, IElementWithValue
{
    private readonly ImmutableArray<IValue> _values;

    internal PropertySymbol(StringRef name, ISymbol parent, Document document, ImmutableArray<IValue> values)
    {
        Name = name;
        Parent = parent;
        Document = document;
        _values = values;

        Attributes = ImmutableDictionary<StringRef, AttributeElement>.Empty;
    }

    public IValue Value => _values[^1];

    public ImmutableDictionary<StringRef, AttributeElement> Attributes { get; internal set; }
    public StringRef Name { get; }
    public ISymbol Parent { get; }
    public Document Document { get; }

    public T Accept<T>(IElementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
