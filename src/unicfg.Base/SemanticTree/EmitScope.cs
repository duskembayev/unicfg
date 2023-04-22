using unicfg.Base.Primitives;

namespace unicfg.Base.SemanticTree;

public sealed class EmitScope : EmitSymbol
{
    private readonly SortedDictionary<StringRef, EmitProperty> _properties;
    private readonly SortedDictionary<StringRef, EmitScope> _scopes;

    public EmitScope() : this(StringRef.Empty)
    {
    }

    public EmitScope(StringRef name) : base(name)
    {
        _scopes = new SortedDictionary<StringRef, EmitScope>();
        _properties = new SortedDictionary<StringRef, EmitProperty>();
    }

    public IReadOnlyDictionary<StringRef, EmitScope> Scopes => _scopes;
    public IReadOnlyDictionary<StringRef, EmitProperty> Properties => _properties;

    public EmitScope? GetScope(StringRef name)
    {
        if (name.IsEmpty)
        {
            throw new InvalidOperationException();
        }

        if (_properties.ContainsKey(name))
        {
            return null;
        }

        if (_scopes.TryGetValue(name, out var scope))
        {
            return scope;
        }

        return _scopes[name] = new EmitScope(name) { Parent = this };
    }

    public EmitProperty? GetProperty(StringRef name)
    {
        if (name.IsEmpty)
        {
            throw new InvalidOperationException();
        }

        if (_scopes.ContainsKey(name))
        {
            return null;
        }

        if (_properties.TryGetValue(name, out var property))
        {
            return property;
        }

        return _properties[name] = new EmitProperty(name) { Parent = this };
    }

    public override ValueTask AcceptAsync(IEmitAsyncVisitor visitor, CancellationToken cancellationToken)
    {
        return visitor.VisitScopeAsync(this, cancellationToken);
    }
}
