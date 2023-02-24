namespace ns2x.Model.Semantic;

public sealed class CollectionValue : IValue
{
    public CollectionValue(ImmutableArray<IValue> values)
    {
        Values = values;
    }

    public ImmutableArray<IValue> Values { get; }

    public void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}