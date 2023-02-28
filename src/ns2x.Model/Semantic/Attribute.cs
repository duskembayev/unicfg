namespace ns2x.Model.Semantic;

public sealed class Attribute : SemanticNodeWithValue
{
    public Attribute(StringRef name, ImmutableArray<IValue> values) : base(name, values)
    {
    }

    public override void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}