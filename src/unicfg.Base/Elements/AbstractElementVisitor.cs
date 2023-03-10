using unicfg.Base.Elements.Values;

namespace unicfg.Base.Elements;

public abstract class AbstractElementVisitor : IElementVisitor
{
    public virtual void Visit(Document document)
    {
        document.RootGroup.Accept(this);
    }

    public virtual void Visit(UniPropertyGroup group)
    {
        foreach (var attribute in group.Attributes) attribute.Accept(this);

        foreach (var property in group.Properties) property.Accept(this);

        foreach (var propertyGroup in group.PropertyGroups) propertyGroup.Accept(this);
    }

    public virtual void Visit(UniProperty property)
    {
        foreach (var attribute in property.Attributes) attribute.Accept(this);

        property.Value.Accept(this);
    }

    public virtual void Visit(UniAttribute attribute)
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