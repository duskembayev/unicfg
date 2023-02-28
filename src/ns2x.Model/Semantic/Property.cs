namespace ns2x.Model.Semantic;

public sealed class Property : SemanticNodeWithValue
{
    public Property(StringRef name, ImmutableArray<Attribute> attributes, ImmutableArray<IValue> values)
        : base(name, values)
    {
        Attributes = attributes;
    }

    public ImmutableArray<Attribute> Attributes { get; }

    public override void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}