using unicfg.Base.Analysis;
using unicfg.Base.Primitives;
using unicfg.Base.SyntaxTree;
using unicfg.Base.SyntaxTree.Values;

namespace unicfg.Uni.Tree.Builders;

internal sealed class SymbolBuilder : IValueOwner
{
    private readonly Dictionary<StringRef, AttributeBuilder> _attributes;
    private readonly Dictionary<StringRef, SymbolBuilder> _children;
    private readonly Diagnostics _diagnostics;
    private readonly StringRef _name;
    private readonly ImmutableArray<IValue>.Builder _values;
    private SymbolKind _kind;

    public SymbolBuilder(StringRef name, SymbolKind kind, Diagnostics diagnostics)
    {
        _name = name;
        _kind = kind;
        _diagnostics = diagnostics;
        _children = new Dictionary<StringRef, SymbolBuilder>();
        _attributes = new Dictionary<StringRef, AttributeBuilder>();
        _values = ImmutableArray.CreateBuilder<IValue>(1);
    }

    public void SetValue(IValue value)
    {
        if (_kind == SymbolKind.Auto)
            _kind = SymbolKind.Property;

        if (_kind == SymbolKind.PropertyGroup)
            _diagnostics.Report(
                DiagnosticDescriptor.UnexpectedValueDeclaration,
                value.SourceRange,
                new object?[] { _name });

        _values.Add(value);
    }

    public AttributeBuilder AddAttribute(in StringRef name)
    {
        if (!_attributes.TryGetValue(name, out var attribute))
        {
            attribute = new AttributeBuilder(name);
            _attributes[name] = attribute;
        }

        return attribute;
    }

    public SymbolBuilder AddSymbol(in StringRef name, in Range sourceRange)
    {
        if (_kind == SymbolKind.Auto)
            _kind = SymbolKind.PropertyGroup;

        if (_kind == SymbolKind.Property)
            _diagnostics.Report(
                DiagnosticDescriptor.UnexpectedSymbolDeclaration,
                sourceRange,
                new object?[] { _name });

        if (!_children.TryGetValue(name, out var child))
        {
            child = new SymbolBuilder(name, SymbolKind.Auto, _diagnostics);
            _children[name] = child;
        }

        return child;
    }

    public IElement? Build(Document document, AbstractSymbol parent)
    {
        return _kind switch
        {
            SymbolKind.PropertyGroup => BuildAsNamespace(document, parent),
            SymbolKind.Property => BuildAsProperty(document, parent),
            _ => null
        };
    }

    public ScopeSymbol BuildAsNamespace(Document document, AbstractSymbol? parent)
    {
        if (_kind != SymbolKind.PropertyGroup)
            throw new InvalidOperationException();

        var result = new ScopeSymbol(_name, document, parent);
        var namespaces = ImmutableArray.CreateBuilder<ScopeSymbol>(_children.Count);
        var properties = ImmutableArray.CreateBuilder<PropertySymbol>(_children.Count);

        foreach (var (_, child) in _children)
        {
            var node = child.Build(document, result);

            if (node is null)
                continue;

            switch (node)
            {
                case ScopeSymbol group:
                    namespaces.Add(group);
                    break;
                case PropertySymbol property:
                    properties.Add(property);
                    break;
            }
        }

        result.Attributes = BuildAttributes(document, result);
        result.Scopes = namespaces.ToImmutable();
        result.Properties = properties.ToImmutable();
        return result;
    }

    public PropertySymbol BuildAsProperty(Document document, AbstractSymbol parent)
    {
        if (_kind != SymbolKind.Property)
            throw new InvalidOperationException();

        var result = new PropertySymbol(_name, document, parent);

        result.Attributes = BuildAttributes(document, result);
        result.Values = _values.ToImmutable();
        return result;
    }

    private ImmutableArray<AttributeSymbol> BuildAttributes(Document document, AbstractSymbol parent)
    {
        return _attributes.Count > 0
            ? _attributes.Select(pair => pair.Value.Build(document, parent)).ToImmutableArray()
            : ImmutableArray<AttributeSymbol>.Empty;
    }
}