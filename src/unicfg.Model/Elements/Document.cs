namespace unicfg.Model.Elements;

public sealed class Document : IElement
{
    private readonly Dictionary<SymbolRef,UniProperty?> _properties;
    private UniPropertyGroup? _rootGroup;

    internal Document()
    {
        _properties = new Dictionary<SymbolRef, UniProperty?>();
    }

    public string WorkingDirectory { get; internal set; }

    public UniPropertyGroup RootGroup
    {
        get => _rootGroup ?? throw new InvalidOperationException();
        internal set => _rootGroup = value;
    }

    public void Accept(IElementVisitor visitor)
    {
        visitor.Visit(this);
    }
    
    public UniProperty? FindProperty(SymbolRef propertyRef)
    {
        if (_properties.TryGetValue(propertyRef, out var property))
            return property;

        property = _properties[propertyRef] = FindPropertyCore(propertyRef);
        return property;
    }

    private UniProperty? FindPropertyCore(SymbolRef propertyRef)
    {
        var group = RootGroup;
        var names = new Queue<StringRef>(propertyRef.Path);

        while (group != null && names.TryDequeue(out var name))
        {
            if (names.Count == 0)
                return group.Properties.SingleOrDefault(p => p.Name.Equals(name));

            group = group.PropertyGroups.SingleOrDefault(n => n.Name.Equals(name));
        }

        return null;
    }
}