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
        {
            _kind = SymbolKind.Property;
        }

        if (_kind == SymbolKind.Scope)
        {
            _diagnostics.Report(
                DiagnosticDescriptor.UnexpectedValueDeclaration,
                value.SourceRange,
                new object?[] { _name });
        }

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
        {
            _kind = SymbolKind.Scope;
        }

        if (_kind == SymbolKind.Property)
        {
            _diagnostics.Report(
                DiagnosticDescriptor.UnexpectedSymbolDeclaration,
                sourceRange,
                new object?[] { _name });
        }

        if (!_children.TryGetValue(name, out var child))
        {
            child = new SymbolBuilder(name, SymbolKind.Auto, _diagnostics);
            _children[name] = child;
        }

        return child;
    }

    public ISymbol Build(Document document, ISymbol? parent)
    {
        return _kind switch
        {
            SymbolKind.Auto => BuildAsScope(document, parent),
            SymbolKind.Scope => BuildAsScope(document, parent),
            SymbolKind.Property => BuildAsProperty(document, parent),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private ScopeSymbol BuildAsScope(Document document, ISymbol? parent)
    {
        var result = new ScopeSymbol(_name, parent, document);
        result.Attributes = BuildAttributes(result);
        result.Children = BuildChildren(result, document);
        return result;
    }

    private PropertySymbol BuildAsProperty(Document document, ISymbol? parent)
    {
        ArgumentNullException.ThrowIfNull(parent);

        var result = new PropertySymbol(_name, parent, document, _values.ToImmutable());
        result.Attributes = BuildAttributes(result);
        return result;
    }

    private ImmutableDictionary<StringRef, ISymbol> BuildChildren(ISymbol parent, Document document)
    {
        if (_children.Count == 0)
        {
            return ImmutableDictionary<StringRef, ISymbol>.Empty;
        }

        return _children.ToImmutableDictionary(
            p => p.Key,
            p => p.Value.Build(document, parent));
    }

    private ImmutableDictionary<StringRef, AttributeElement> BuildAttributes(ISymbol parent)
    {
        if (_attributes.Count == 0)
        {
            return ImmutableDictionary<StringRef, AttributeElement>.Empty;
        }

        return _attributes.ToImmutableDictionary(
            p => p.Key,
            p => p.Value.Build(parent));
    }
}
