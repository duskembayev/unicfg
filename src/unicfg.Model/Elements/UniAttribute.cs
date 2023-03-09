namespace unicfg.Model.Elements;

public sealed class UniAttribute : ElementWithValue
{
    public UniAttribute(StringRef name, Document document, ElementWithName parent)
        : base(name, document, parent)
    {
    }

    public override void Accept(IElementVisitor visitor)
    {
        visitor.Visit(this);
    }
}