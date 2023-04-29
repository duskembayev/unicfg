using unicfg.Base.Extensions;
using unicfg.Base.SyntaxTree.Walkers;
using unicfg.Evaluation.Extensions;

namespace unicfg.Evaluation.Walkers;

internal class EmitScopeBuilder : AsyncWalker
{
    private readonly CancellationToken _cancellationToken;
    private readonly IValueEvaluator _valueEvaluator;
    private readonly IDiagnostics _diagnostics;
    private EmitSymbol _currentSymbol;

    public EmitScopeBuilder(
        IValueEvaluator valueEvaluator,
        ImmutableDictionary<SymbolRef, StringRef> defaults,
        IDiagnostics diagnostics,
        CancellationToken cancellationToken)
        : base(cancellationToken)
    {
        _valueEvaluator = valueEvaluator;
        _diagnostics = diagnostics;
        _cancellationToken = cancellationToken;

        _currentSymbol = Scope = new EmitScope();

        foreach (var (key, value) in defaults)
        {
            Scope.SetPropertyValue(key, EmitValue.CreateEvaluatedValue(value));
        }
    }

    public EmitScope Scope { get; }

    public override async ValueTask Visit(ScopeSymbol scope)
    {
        var child = _currentSymbol switch
        {
            EmitScope { Parent: null } parent when scope.IsRoot => parent,
            EmitScope parent => parent.ResolveScope(scope.Name),
            _ => null
        };

        if (child is null)
        {
            _diagnostics.Report(Conflict, new object[] { scope.GetSymbolRef() });
            return;
        }

        _currentSymbol = child;
        await base.Visit(scope).ConfigureAwait(false);
        _currentSymbol = child.Parent!;
    }

    public override async ValueTask Visit(PropertySymbol property)
    {
        var child = _currentSymbol switch
        {
            EmitScope parent => parent.ResolveProperty(property.Name),
            _ => null
        };

        if (child is null)
        {
            _diagnostics.Report(Conflict, new object[] { property.GetSymbolRef() });
            return;
        }

        _currentSymbol = child;

        foreach (var (_, element) in property.Attributes)
        {
            await element.Accept(this).ConfigureAwait(false);
        }

        var value = await _valueEvaluator
            .EvaluateAsync(property, _cancellationToken)
            .ConfigureAwait(false);

        ((EmitProperty)_currentSymbol).Value = value;
        _currentSymbol = _currentSymbol.Parent!;
    }

    public override async ValueTask Visit(AttributeElement attribute)
    {
        var value = await _valueEvaluator
            .EvaluateAsync(attribute, _cancellationToken)
            .ConfigureAwait(false);

        _currentSymbol.SetAttributeValue(attribute.Name, value);
    }
}
