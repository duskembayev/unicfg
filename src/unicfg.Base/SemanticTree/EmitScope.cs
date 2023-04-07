using unicfg.Base.Primitives;

namespace unicfg.Base.SemanticTree;

public sealed class EmitScope
{
    private readonly Dictionary<StringRef, EmitScope> _scopes;
    private readonly Dictionary<StringRef, EmitProperty> _properties;
    private readonly Dictionary<StringRef, StringRef> _attributes;

    public EmitScope()
    {
        _scopes = new Dictionary<StringRef, EmitScope>();
        _properties = new Dictionary<StringRef, EmitProperty>();
        _attributes = new Dictionary<StringRef, StringRef>();
    }

    public StringRef? Name { get; init; }
    public EmitScope? Parent { get; init; }

    public IReadOnlyDictionary<StringRef, EmitScope> Scopes => _scopes;
    public IReadOnlyDictionary<StringRef, EmitProperty> Properties => _properties;
    public IReadOnlyDictionary<StringRef, StringRef> Attributes => _attributes;

    public EmitScope? Scope(StringRef name)
    {
        if (_properties.ContainsKey(name))
            return null;

        if (_scopes.TryGetValue(name, out var scope))
            return scope;

        return _scopes[name] = new EmitScope
        {
            Name = name,
            Parent = this
        };
    }

    public EmitProperty? Property(StringRef name)
    {
        if (_scopes.ContainsKey(name))
            return null;

        if (_properties.TryGetValue(name, out var property))
            return property;

        return _properties[name] = new EmitProperty
        {
            Name = name,
            Parent = this,
        };
    }

    public void Argument(StringRef name, StringRef value)
    {
        _attributes[name] = value;
    }
}