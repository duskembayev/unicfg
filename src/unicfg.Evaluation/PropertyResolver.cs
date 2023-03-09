namespace unicfg.Evaluation;

public sealed class PropertyResolver : IPropertyResolver
{
    private readonly Document _document;

    public PropertyResolver(Document document)
    {
        _document = document;
    }

    public UniProperty? ResolveProperty(PropertyRef propertyRef)
    {
        return _document.FindProperty(propertyRef);
    }
}