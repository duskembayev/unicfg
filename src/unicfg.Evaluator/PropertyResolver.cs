using unicfg.Model.Primitives;
using unicfg.Model.Semantic;

namespace unicfg.Evaluator;

public sealed class PropertyResolver : IPropertyResolver
{
    private readonly Document _document;
    private readonly Dictionary<PropertyRef, Property?> _properties;

    public PropertyResolver(Document document)
    {
        _document = document;
        _properties = new Dictionary<PropertyRef, Property?>();
    }
    
    public Property? ResolveProperty(PropertyRef propertyRef)
    {
        if (_properties.TryGetValue(propertyRef, out var property))
            return property;

        property = _properties[propertyRef] = ResolveCore(propertyRef);
        return property;
    }

    private Property? ResolveCore(PropertyRef propertyRef)
    {
        var ns = _document.RootNamespace;
        var names = new Queue<StringRef>(propertyRef.Path);

        while (ns != null && names.TryDequeue(out var name))
        {
            if (names.Count == 0)
                return ns.Properties.SingleOrDefault(p => p.Name.Equals(name));

            ns = ns.Namespaces.SingleOrDefault(n => n.Name.Equals(name));
        }

        return null;
    }
}