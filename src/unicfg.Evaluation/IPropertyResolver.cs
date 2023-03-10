using unicfg.Base.Elements;
using unicfg.Base.Primitives;

namespace unicfg.Evaluation;

public interface IPropertyResolver
{
    UniProperty? ResolveProperty(SymbolRef propertyRef);
}