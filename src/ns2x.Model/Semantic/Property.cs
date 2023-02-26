namespace ns2x.Model.Semantic;

public sealed class Property : ISemanticNodeWithValue
{
    public Property(StringRef name, ImmutableArray<IValue> values, ImmutableArray<Attribute> attributes)
    {
        Name = name;
        Values = values;
        Attributes = attributes;
    }

    public StringRef Name { get; }

    public ImmutableArray<IValue> Values { get; }

    public ImmutableArray<Attribute> Attributes { get; }

    public void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}