using unicfg.Base.Primitives;

namespace unicfg.Base.SyntaxTree;

public sealed class ScopeSymbol : AbstractSymbol
{
    public ScopeSymbol(StringRef name, Document document, AbstractSymbol? parent)
        : base(name, document, parent)
    {
    }

    public ImmutableArray<ScopeSymbol> Scopes { get; internal set; }
    public ImmutableArray<PropertySymbol> Properties { get; internal set; }
    public ImmutableArray<AttributeSymbol> Attributes { get; internal set; }

    public override void Accept(IElementVisitor visitor)
    {
        visitor.Visit(this);
    }
}