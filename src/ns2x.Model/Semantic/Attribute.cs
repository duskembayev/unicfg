namespace ns2x.Model.Semantic;

public sealed class Attribute : ISemanticNodeWithValue
{
    public Attribute(StringRef name, ImmutableArray<IValue> values)
    {
        Name = name;
        Values = values;
    }

    public StringRef Name { get; }

    public ImmutableArray<IValue> Values { get; }

    public void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}