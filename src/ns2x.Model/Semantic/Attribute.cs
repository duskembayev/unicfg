namespace ns2x.Model.Semantic;

public sealed class Attribute : ISemanticNode
{
    public Attribute(StringRef name, IValue value)
    {
        Name = name;
        Value = value;
    }

    public StringRef Name { get; }

    public IValue Value { get; }

    public void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}