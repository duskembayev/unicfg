using unicfg.Base.Primitives;

namespace unicfg.Base.SyntaxTree;

public sealed class AttributeSymbol : AbstractSymbolWithValue
{
    public AttributeSymbol(StringRef name, Document document, AbstractSymbol parent)
        : base(name, document, parent)
    {
    }

    public override void Accept(IElementVisitor visitor)
    {
        visitor.Visit(this);
    }
}