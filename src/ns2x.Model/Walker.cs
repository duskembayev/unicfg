using ns2x.Model.Semantic;
using Attribute = ns2x.Model.Semantic.Attribute;

namespace ns2x.Model;

public abstract class Walker : ISemanticNodeVisitor
{
    public virtual void Visit(Document document)
    {
        document.RootNamespace.Accept(this);
    }

    public virtual void Visit(Namespace ns)
    {
        foreach (var attribute in ns.Attributes) attribute.Accept(this);

        foreach (var property in ns.Properties) property.Accept(this);

        foreach (var sns in ns.Namespaces) sns.Accept(this);
    }

    public virtual void Visit(Property property)
    {
        foreach (var attribute in property.Attributes) attribute.Accept(this);

        foreach (var value in property.Values) value.Accept(this);
    }

    public virtual void Visit(Attribute attribute)
    {
        foreach (var value in attribute.Values) value.Accept(this);
    }

    public virtual void Visit(TextValue textValue)
    {
    }

    public virtual void Visit(RefValue refValue)
    {
    }

    public virtual void Visit(EmptyValue emptyValue)
    {
    }

    public virtual void Visit(CollectionValue collectionValue)
    {
        foreach (var value in collectionValue.Values)
            value.Accept(this);
    }
}