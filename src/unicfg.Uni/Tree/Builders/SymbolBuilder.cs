using unicfg.Model.Elements;
using unicfg.Model.Elements.Values;

namespace unicfg.Uni.Tree.Builders;

internal sealed class SymbolBuilder : IValueOwner
{
    private readonly Dictionary<StringRef, AttributeBuilder> _attributes;
    private readonly Dictionary<StringRef, SymbolBuilder> _children;
    private readonly ImmutableArray<IValue>.Builder _values;
    private readonly StringRef _name;
    private SymbolKind _kind;
    private readonly Diagnostics _diagnostics;

    public SymbolBuilder(StringRef name, SymbolKind kind, Diagnostics diagnostics)
    {
        _name = name;
        _kind = kind;
        _diagnostics = diagnostics;
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

    public SymbolBuilder AddSymbol(in StringRef name, in Range sourceRange)
    {
        if (_kind == SymbolKind.Auto)
            _kind = SymbolKind.Namespace;

        if (_kind == SymbolKind.Property)
            _diagnostics.Report(DiagnosticDescriptor.UnexpectedSymbolDeclaration, sourceRange, _name);

        if (!_children.TryGetValue(name, out var ns))
        {
            ns = new SymbolBuilder(name, SymbolKind.Auto, _diagnostics);
            _children[name] = ns;
        }

        return ns;
    }

    public void SetValue(IValue value)
    {
        if (_kind == SymbolKind.Auto)
            _kind = SymbolKind.Property;

        if (_kind == SymbolKind.Namespace)
            _diagnostics.Report(DiagnosticDescriptor.UnexpectedValueDeclaration, value.SourceRange, _name);

        _values.Add(value);
    }

    public IElement? Build(Document document, ElementWithName parent)
    {
        return _kind switch
        {
            SymbolKind.Namespace => BuildAsNamespace(document, parent),
            SymbolKind.Property => BuildAsProperty(document, parent),
            _ => null
        };
    }

    public UniPropertyGroup BuildAsNamespace(Document document, ElementWithName? parent)
    {
        if (_kind != SymbolKind.Namespace)
            throw new InvalidOperationException();

        var result = new UniPropertyGroup(_name, document, parent);
        var namespaces = ImmutableArray.CreateBuilder<UniPropertyGroup>(_children.Count);
        var properties = ImmutableArray.CreateBuilder<UniProperty>(_children.Count);

        foreach (var (_, child) in _children)
        {
            var node = child.Build(document, result);

            if (node is null)
                continue;

            switch (node)
            {
                case UniPropertyGroup ns:
                    namespaces.Add(ns);
                    break;
                case UniProperty property:
                    properties.Add(property);
                    break;
            }
        }

        result.Attributes = BuildAttributes(document, result);
        result.Subgroups = namespaces.ToImmutable();
        result.Properties = properties.ToImmutable();
        return result;
    }

    public UniProperty BuildAsProperty(Document document, ElementWithName parent)
    {
        if (_kind != SymbolKind.Property)
            throw new InvalidOperationException();

        var result = new UniProperty(_name, document, parent);

        result.Attributes = BuildAttributes(document, result);
        result.Values = _values.ToImmutable();
        return result;
    }

    private ImmutableArray<UniAttribute> BuildAttributes(Document document, ElementWithName parent)
    {
        return _attributes.Count > 0
            ? _attributes.Select(pair => pair.Value.Build(document, parent)).ToImmutableArray()
            : ImmutableArray<UniAttribute>.Empty;
    }
}