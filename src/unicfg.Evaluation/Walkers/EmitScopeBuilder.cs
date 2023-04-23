using unicfg.Base.SyntaxTree.Walkers;
using unicfg.Evaluation.Extensions;

namespace unicfg.Evaluation.Walkers;

internal class EmitScopeBuilder : AsyncWalker
{
    private readonly CancellationToken _cancellationToken;
    private readonly IValueEvaluator _valueEvaluator;
    private EmitSymbol? _currentSymbol;

    public EmitScopeBuilder(
        IValueEvaluator valueEvaluator,
        ImmutableDictionary<SymbolRef, StringRef> defaults,
        CancellationToken cancellationToken)
        : base(cancellationToken)
    {
        _valueEvaluator = valueEvaluator;
        _cancellationToken = cancellationToken;

        Scope = new EmitScope();

        foreach (var (key, value) in defaults)
        {
            Scope.SetPropertyValue(key, EmitValue.CreateEvaluatedValue(value));
        }
    }

    public EmitScope Scope { get; }

    public override async ValueTask Visit(ScopeSymbol scope)
    {
        _currentSymbol = _currentSymbol switch
        {
            null => Scope,
            EmitScope parent => parent.GetScope(scope.Name),
            _ => null
        };

        if (_currentSymbol is null)
        {
            throw new NotImplementedException();
        }

        await base.Visit(scope).ConfigureAwait(false);
        _currentSymbol = _currentSymbol.Parent;
    }

    public override async ValueTask Visit(PropertySymbol property)
    {
        _currentSymbol = _currentSymbol switch
        {
            EmitScope parent => parent.GetProperty(property.Name),
            _ => null
        };

        if (_currentSymbol is null)
        {
            throw new NotImplementedException();
        }

        foreach (var (_, element) in property.Attributes)
        {
            await element.Accept(this).ConfigureAwait(false);
        }

        var value = await _valueEvaluator
            .EvaluateAsync(property, _cancellationToken)
            .ConfigureAwait(false);

        ((EmitProperty)_currentSymbol).Value = value;
        _currentSymbol = _currentSymbol.Parent;
    }

    public override async ValueTask Visit(AttributeElement attribute)
    {
        if (_currentSymbol is null)
        {
            throw new NotSupportedException("Unable to evaluate attribute outside of a property or scope.");
        }

        var value = await _valueEvaluator
            .EvaluateAsync(attribute, _cancellationToken)
            .ConfigureAwait(false);

        _currentSymbol.SetAttributeValue(attribute.Name, value);
    }
}
