using unicfg.Base.Primitives;
using unicfg.Base.SyntaxTree.Values;

namespace unicfg.Base.SyntaxTree;

public sealed class AttributeElement : INamedElement, IElementWithValue
{
    private readonly ImmutableArray<IValue> _values;

    internal AttributeElement(StringRef name, ISymbol parent, ImmutableArray<IValue> values)
    {
        Name = name;
        Parent = parent;
        _values = values;
    }

    public IValue Value => _values[^1];
    public ISymbol Parent { get; }
    public StringRef Name { get; }

    public T Accept<T>(IElementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
