using unicfg.Model.Primitives;
using unicfg.Model.Semantic;

namespace unicfg.Parser.Builders;

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

    public ISemanticNode? Build(Document document, SemanticNodeWithName parent)
    {
        return _kind switch
        {
            SymbolKind.Namespace => BuildAsNamespace(document, parent),
            SymbolKind.Property => BuildAsProperty(document, parent),
            _ => null
        };
    }

    public Namespace BuildAsNamespace(Document document, SemanticNodeWithName? parent)
    {
        if (_kind != SymbolKind.Namespace)
            throw new InvalidOperationException();

        var result = new Namespace(_name, document, parent);
        var namespaces = ImmutableArray.CreateBuilder<Namespace>(_children.Count);
        var properties = ImmutableArray.CreateBuilder<Property>(_children.Count);

        foreach (var (_, child) in _children)
        {
            var node = child.Build(document, result);

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

        result.Attributes = BuildAttributes(document, result);
        result.Namespaces = namespaces.ToImmutable();
        result.Properties = properties.ToImmutable();
        return result;
    }

    public Property BuildAsProperty(Document document, SemanticNodeWithName parent)
    {
        if (_kind != SymbolKind.Property)
            throw new InvalidOperationException();

        var result = new Property(_name, document, parent);

        result.Attributes = BuildAttributes(document, result);
        result.Values = _values.ToImmutable();
        return result;
    }

    private ImmutableArray<Attribute> BuildAttributes(Document document, SemanticNodeWithName parent)
    {
        return _attributes.Count > 0
            ? _attributes.Select(pair => pair.Value.Build(document, parent)).ToImmutableArray()
            : ImmutableArray<Attribute>.Empty;
    }
}