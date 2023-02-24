namespace ns2x.Model.Semantic;

public sealed class Property : ISemanticNode
{
    public Property(StringRef name, IValue value, ImmutableArray<Attribute> attributes)
    {
        Name = name;
        Value = value;
        Attributes = attributes;
    }

    public StringRef Name { get; }

    public IValue Value { get; }

    public ImmutableArray<Attribute> Attributes { get; }

    public void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}