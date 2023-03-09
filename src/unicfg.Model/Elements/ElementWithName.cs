namespace unicfg.Model.Elements;

public abstract class ElementWithName : IElement
{
    protected ElementWithName(StringRef name, Document document, ElementWithName? parent)
    {
        Name = name;
        Document = document;
        Parent = parent;
    }

    public StringRef Name { get; }
    public Document Document { get; }
    public ElementWithName? Parent { get; }

    public abstract void Accept(IElementVisitor visitor);
}