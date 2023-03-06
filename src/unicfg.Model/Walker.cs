using unicfg.Model.Semantic;
using Attribute = unicfg.Model.Semantic.Attribute;

namespace unicfg.Model;

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

        property.Value.Accept(this);
    }

    public virtual void Visit(Attribute attribute)
    {
        attribute.Value.Accept(this);
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

    public virtual void Visit(ErrorValue errorValue)
    {
    }

    public virtual void Visit(CollectionValue collectionValue)
    {
        foreach (var value in collectionValue.Values)
            value.Accept(this);
    }
}