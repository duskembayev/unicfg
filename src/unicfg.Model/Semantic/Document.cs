using unicfg.Model.Primitives;

namespace unicfg.Model.Semantic;

public class Document : ISemanticNode
{
    private readonly Dictionary<PropertyRef,Property?> _properties;
    private Namespace? _rootNamespace;

    internal Document()
    {
        _properties = new Dictionary<PropertyRef, Property?>();
    }

    public Namespace RootNamespace
    {
        get => _rootNamespace ?? throw new InvalidOperationException();
        internal set => _rootNamespace = value;
    }

    public void Accept(ISemanticNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
    
    public Property? FindProperty(PropertyRef propertyRef)
    {
        if (_properties.TryGetValue(propertyRef, out var property))
            return property;

        property = _properties[propertyRef] = FindPropertyCore(propertyRef);
        return property;
    }

    private Property? FindPropertyCore(PropertyRef propertyRef)
    {
        var ns = RootNamespace;
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