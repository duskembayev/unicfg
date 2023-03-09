using unicfg.Model.Elements;
using unicfg.Model.Primitives;

namespace unicfg.Evaluation;

public interface IPropertyResolver
{
    UniProperty? ResolveProperty(PropertyRef propertyRef);
}