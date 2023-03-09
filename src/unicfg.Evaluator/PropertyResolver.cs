using unicfg.Model.Primitives;
using unicfg.Model.Semantic;

namespace unicfg.Evaluator;

public sealed class PropertyResolver : IPropertyResolver
{
    private readonly Document _document;

    public PropertyResolver(Document document)
    {
        _document = document;
    }

    public Property? ResolveProperty(PropertyRef propertyRef)
    {
        return _document.FindProperty(propertyRef);
    }
}