using unicfg.Base.Primitives;

namespace unicfg.Base.Elements;

public sealed class Document : IElement
{
    private readonly Dictionary<SymbolRef,UniProperty?> _properties;
    private UniScope? _rootGroup;

    internal Document(string baseDirectory, string? location)
    {
        BaseDirectory = baseDirectory;
        Location = location;

        _properties = new Dictionary<SymbolRef, UniProperty?>();
    }

    public string? Location { get; }
    public string BaseDirectory { get; }

    public UniScope RootScope
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
        var group = RootScope;
        var names = new Queue<StringRef>(propertyRef.Path);

        while (group != null && names.TryDequeue(out var name))
        {
            if (names.Count == 0)
                return group.Properties.SingleOrDefault(p => p.Name.Equals(name));

            group = group.Scopes.SingleOrDefault(n => n.Name.Equals(name));
        }

        return null;
    }
}