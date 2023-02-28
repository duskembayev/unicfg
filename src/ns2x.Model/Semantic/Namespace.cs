namespace ns2x.Model.Semantic;

public sealed class Namespace : ISemanticNodeWithName
{
    public Namespace(StringRef name, ImmutableArray<Namespace> namespaces, ImmutableArray<Property> properties, ImmutableArray<Attribute> attributes)
    {
        Name = name;
        Namespaces = namespaces;
        Properties = properties;
        Attributes = attributes;
    }

    public StringRef Name { get; }

    public ImmutableArray<Namespace> Namespaces { get; }
    public ImmutableArray<Property> Properties { get; }
    public ImmutableArray<Attribute> Attributes { get; }

    public void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}