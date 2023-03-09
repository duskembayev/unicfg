namespace unicfg.Model.Elements;

public sealed class UniProperty : ElementWithValue
{
    public UniProperty(StringRef name, Document document, ElementWithName parent)
        : base(name, document, parent)
    {
    }

    public ImmutableArray<UniAttribute> Attributes { get; internal set; }

    public override void Accept(IElementVisitor visitor)
    {
        visitor.Visit(this);
    }
}