using unicfg.Base.Elements;
using unicfg.Base.Primitives;

namespace unicfg.Evaluation;

public sealed class PropertyResolver : IPropertyResolver
{
    private readonly Document _document;

    public PropertyResolver(Document document)
    {
        _document = document;
    }

    public UniProperty? ResolveProperty(SymbolRef propertyRef)
    {
        return _document.FindProperty(propertyRef);
    }
}