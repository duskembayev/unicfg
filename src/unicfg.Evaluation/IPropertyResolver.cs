namespace unicfg.Evaluation;

public interface IPropertyResolver
{
    PropertySymbol? ResolveProperty(SymbolRef propertyRef);
}
