namespace ns2x.Model.Semantic;

public sealed class Property : SemanticNodeWithValue
{
    public Property(StringRef name, Document document, SemanticNodeWithName parent)
        : base(name, document, parent)
    {
    }

    public ImmutableArray<Attribute> Attributes { get; internal set; }

    public override void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}