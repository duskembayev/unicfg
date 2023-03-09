namespace unicfg.Model.Elements;

public sealed class Document : IElement
{
    private readonly Dictionary<PropertyRef,UniProperty?> _properties;
    private UniPropertyGroup? _rootGroup;

    internal Document()
    {
        _properties = new Dictionary<PropertyRef, UniProperty?>();
    }

    public UniPropertyGroup RootGroup
    {
        get => _rootGroup ?? throw new InvalidOperationException();
        internal set => _rootGroup = value;
    }

    public void Accept(IElementVisitor visitor)
    {
        visitor.Visit(this);
    }
    
    public UniProperty? FindProperty(PropertyRef propertyRef)
    {
        if (_properties.TryGetValue(propertyRef, out var property))
            return property;

        property = _properties[propertyRef] = FindPropertyCore(propertyRef);
        return property;
    }

    private UniProperty? FindPropertyCore(PropertyRef propertyRef)
    {
        var group = RootGroup;
        var names = new Queue<StringRef>(propertyRef.Path);

        while (group != null && names.TryDequeue(out var name))
        {
            if (names.Count == 0)
                return group.Properties.SingleOrDefault(p => p.Name.Equals(name));

            group = group.Subgroups.SingleOrDefault(n => n.Name.Equals(name));
        }

        return null;
    }
}