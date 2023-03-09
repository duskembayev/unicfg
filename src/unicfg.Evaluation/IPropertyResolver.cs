namespace unicfg.Evaluation;

public interface IPropertyResolver
{
    UniProperty? ResolveProperty(PropertyRef propertyRef);
}