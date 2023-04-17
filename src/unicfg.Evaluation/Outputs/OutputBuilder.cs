using unicfg.Base.SemanticTree;
using unicfg.Base.SyntaxTree;
using unicfg.Evaluation.Walkers;

namespace unicfg.Evaluation.Outputs;

internal class OutputBuilder : AsyncWalker
{
    private readonly IValueEvaluator _valueEvaluator;
    private readonly CancellationToken _cancellationToken;
    private EmitSymbol? _currentSymbol;

    public OutputBuilder(IValueEvaluator valueEvaluator, CancellationToken cancellationToken) : base(cancellationToken)
    {
        _valueEvaluator = valueEvaluator;
        _cancellationToken = cancellationToken;

        Scope = new EmitScope();
    }

    public EmitScope Scope { get; }

    public override async ValueTask Visit(ScopeSymbol scope)
    {
        _currentSymbol = _currentSymbol switch
        {
            null when scope.IsRoot => Scope,
            EmitScope parent => parent.GetScope(scope.Name),
            _ => null
        };

        if (_currentSymbol is null)
            throw new NotImplementedException();

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
            throw new NotImplementedException();

        foreach (var (_, element) in property.Attributes)
            await element
                .Accept(this)
                .ConfigureAwait(false);

        var value = await _valueEvaluator
            .EvaluateAsync(property, _cancellationToken)
            .ConfigureAwait(false);
        
        ((EmitProperty) _currentSymbol).Value = value;
        _currentSymbol = _currentSymbol.Parent;
    }

    public override async ValueTask Visit(AttributeElement attribute)
    {
        if (_currentSymbol is null)
            throw new NotImplementedException();

        var value = await _valueEvaluator
            .EvaluateAsync(attribute, _cancellationToken)
            .ConfigureAwait(false);

        _currentSymbol.SetAttributeValue(attribute.Name, value);
    }
}