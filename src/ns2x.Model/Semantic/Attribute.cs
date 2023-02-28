namespace ns2x.Model.Semantic;

public sealed class Attribute : SemanticNodeWithValue
{
    public Attribute(StringRef name, Document document, SemanticNodeWithName parent)
        : base(name, document, parent)
    {
    }

    public override void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}