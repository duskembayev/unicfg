namespace ns2x.Parser.Builders;

internal sealed class SymbolBuilder : IValueOwner
{
    private readonly Dictionary<StringRef, AttributeBuilder> _attributes;
    private readonly Dictionary<StringRef, SymbolBuilder> _children;
    private readonly ImmutableArray<IValue>.Builder _values;
    private readonly StringRef _name;
    private SymbolKind _kind;

    public SymbolBuilder(StringRef name, SymbolKind kind)
    {
        _name = name;
        _kind = kind;
        _children = new Dictionary<StringRef, SymbolBuilder>();
        _attributes = new Dictionary<StringRef, AttributeBuilder>();
        _values = ImmutableArray.CreateBuilder<IValue>(1);
    }

    public AttributeBuilder AddAttribute(in StringRef name)
    {
        if (!_attributes.TryGetValue(name, out var a))
        {
            a = new AttributeBuilder(name);
            _attributes[name] = a;
        }

        return a;
    }

    public SymbolBuilder AddSymbol(in StringRef name)
    {
        if (_kind == SymbolKind.Auto)
            _kind = SymbolKind.Namespace;

        if (!_children.TryGetValue(name, out var ns))
        {
            ns = new SymbolBuilder(name, SymbolKind.Auto);
            _children[name] = ns;
        }

        return ns;
    }

    public void SetValue(IValue value)
    {
        if (_kind == SymbolKind.Auto)
            _kind = SymbolKind.Property;

        _values.Add(value);
    }

    public ISemanticNode? Build()
    {
        return _kind switch
        {
            SymbolKind.Namespace => BuildAsNamespace(),
            SymbolKind.Property => BuildAsProperty(),
            _ => null
        };
    }

    public Property BuildAsProperty()
    {
        if (_kind != SymbolKind.Property)
            throw new InvalidOperationException();

        var attributes = _attributes.Count > 0
            ? _attributes.Select(pair => pair.Value.Build()).ToImmutableArray()
            : ImmutableArray<Attribute>.Empty;

        return new Property(_name, attributes, _values.ToImmutable());
    }

    public Namespace BuildAsNamespace()
    {
        if (_kind != SymbolKind.Namespace)
            throw new InvalidOperationException();

        var attributes = _attributes.Count > 0
            ? _attributes.Select(pair => pair.Value.Build()).ToImmutableArray()
            : ImmutableArray<Attribute>.Empty;

        var namespaces = ImmutableArray.CreateBuilder<Namespace>();
        var properties = ImmutableArray.CreateBuilder<Property>();

        foreach (var (_, child) in _children)
        {
            var node = child.Build();

            if (node is null)
                continue;

            switch (node)
            {
                case Namespace ns:
                    namespaces.Add(ns);
                    break;
                case Property property:
                    properties.Add(property);
                    break;
            }
        }

        return new Namespace(_name, namespaces.ToImmutable(), properties.ToImmutable(), attributes);
    }
}