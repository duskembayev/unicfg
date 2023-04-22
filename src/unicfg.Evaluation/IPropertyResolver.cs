using unicfg.Base.Primitives;
using unicfg.Base.SyntaxTree;

namespace unicfg.Evaluation;

public interface IPropertyResolver
{
    PropertySymbol? ResolveProperty(SymbolRef propertyRef);
}
