namespace ns2x.Model.Semantic;

public sealed class RefValue : IValue
{
    public RefValue(PropertyRef property)
    {
        Property = property;
    }

    public PropertyRef Property { get; }

    public void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}