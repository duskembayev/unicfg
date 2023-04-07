using unicfg.Base.Primitives;

namespace unicfg.Base.SyntaxTree;

public abstract class AbstractSymbol : IElement
{
    protected AbstractSymbol(StringRef name, Document document, AbstractSymbol? parent)
    {
        Name = name;
        Document = document;
        Parent = parent;
    }

    public StringRef Name { get; }
    public Document Document { get; }
    public AbstractSymbol? Parent { get; }

    public abstract void Accept(IElementVisitor visitor);
}