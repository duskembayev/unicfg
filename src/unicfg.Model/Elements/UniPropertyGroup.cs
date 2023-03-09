namespace unicfg.Model.Elements;

public sealed class UniPropertyGroup : UniSymbol
{
    public UniPropertyGroup(StringRef name, Document document, ElementWithName? parent)
        : base(name, document, parent)
    {
    }

    public ImmutableArray<UniPropertyGroup> Subgroups { get; internal set; }
    public ImmutableArray<UniProperty> Properties { get; internal set; }

    public override void Accept(IElementVisitor visitor)
    {
        visitor.Visit(this);
    }
}