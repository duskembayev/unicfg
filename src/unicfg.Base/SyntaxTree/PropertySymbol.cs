using unicfg.Base.Primitives;

namespace unicfg.Base.SyntaxTree;

public sealed class PropertySymbol : AbstractSymbolWithValue
{
    public PropertySymbol(StringRef name, Document document, AbstractSymbol parent)
        : base(name, document, parent)
    {
    }

    public ImmutableArray<AttributeSymbol> Attributes { get; internal set; }

    public override void Accept(IElementVisitor visitor)
    {
        visitor.Visit(this);
    }
}