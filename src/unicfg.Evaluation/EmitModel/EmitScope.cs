using unicfg.Base.Analysis;
using unicfg.Base.Primitives;

namespace unicfg.Evaluation.EmitModel;

public sealed record EmitScope
{
    private readonly EmitScope _parent;
    private readonly Diagnostics _diagnostics;
    private readonly Dictionary<StringRef, EmitScope> _scopes;
    private readonly Dictionary<StringRef, EmitProperty> _properties;
    private readonly Dictionary<StringRef, StringRef> _arguments;

    public EmitScope(EmitScope parent, Diagnostics diagnostics)
    {
        _parent = parent;
        _diagnostics = diagnostics;

        _scopes = new Dictionary<StringRef, EmitScope>();
        _properties = new Dictionary<StringRef, EmitProperty>();
        _arguments = new Dictionary<StringRef, StringRef>();
    }

    public StringRef? Type { get; set; }

    public IReadOnlyDictionary<StringRef, EmitScope> Scopes => _scopes;
    public IReadOnlyDictionary<StringRef, EmitProperty> Properties => _properties;
    public IReadOnlyDictionary<StringRef, StringRef> Arguments => _arguments;

    public EmitScope Scope(StringRef name)
    {
        if (_properties.TryGetValue(name, out var property))
            _diagnostics.Report(DiagnosticDescriptor.SymbolConflict, new object?[] { _parent. });
    }

    public void Argument(StringRef name, StringRef value)
    {
        _arguments[name] = value;
        S
    }
}

public sealed record EmitProperty
{
    public IReadOnlyDictionary<StringRef, StringRef> Arguments { get; set; }
    public StringRef Value { get; set; }
}