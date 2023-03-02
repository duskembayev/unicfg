using unicfg.Model.Primitives;
using unicfg.Model.Semantic;

namespace unicfg.Evaluator;

public interface IPropertyResolver
{
    Property? ResolveProperty(PropertyRef propertyRef);
}