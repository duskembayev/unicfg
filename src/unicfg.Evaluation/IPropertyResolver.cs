namespace unicfg.Evaluation;

public interface IPropertyResolver
{
    UniProperty? ResolveProperty(SymbolRef propertyRef);
}