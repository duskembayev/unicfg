using ns2x.Model.Primitives;
using ns2x.Model.Semantic;

namespace ns2x.Evaluator;

public interface IPropertyResolver
{
    Property? ResolveProperty(PropertyRef propertyRef);
}