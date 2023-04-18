using unicfg.Base.Primitives;
using unicfg.Base.SemanticTree;
using unicfg.Base.SyntaxTree.Values;
using unicfg.Evaluation.Walkers;

namespace unicfg.Evaluation.Outputs;

internal sealed class ValueBuilder : AsyncWalker
{
    private readonly IReadOnlyDictionary<SymbolRef, EmitValue> _dependencies;
    private readonly CancellationToken _cancellationToken;

    private SymbolRef _unresolvedDependency;
    private bool _hasErrors;
    private StringRef _value;

    public ValueBuilder(IReadOnlyDictionary<SymbolRef, EmitValue> dependencies, CancellationToken cancellationToken)
        : base(cancellationToken)
    {
        _dependencies = dependencies;
        _cancellationToken = cancellationToken;
        _unresolvedDependency = SymbolRef.Null;
    }

    public SymbolRef UnresolvedDependency => _unresolvedDependency;

    public EmitValue? GetResult()
    {
        if (_hasErrors)
            return EmitValue.Error;

        if (_unresolvedDependency != SymbolRef.Null)
            return null;

        return EmitValue.CreateEvaluatedValue(_value);
    }

    public void Reset()
    {
        _unresolvedDependency = SymbolRef.Null;
        _hasErrors = false;
        _value = StringRef.Empty;
    }

    public override ValueTask Visit(TextValue textValue)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        if (_hasErrors)
            return ValueTask.CompletedTask;

        if (_unresolvedDependency != SymbolRef.Null)
            return ValueTask.CompletedTask;
        
        _value += textValue.Text;
        return ValueTask.CompletedTask;
    }

    public override ValueTask Visit(ErrorValue errorValue)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        _hasErrors = true;
        _value = StringRef.Empty;
        return ValueTask.CompletedTask;
    }

    public override ValueTask Visit(RefValue refValue)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        if (_hasErrors)
            return ValueTask.CompletedTask;

        if (_unresolvedDependency != SymbolRef.Null)
            return ValueTask.CompletedTask;

        if (!_dependencies.TryGetValue(refValue.Property, out var value))
        {
            _unresolvedDependency = refValue.Property;
            _value = StringRef.Empty;
            return ValueTask.CompletedTask;
        }

        if (value.State is EvaluationState.Error)
        {
            _hasErrors = true;
            _value = StringRef.Empty;
            return ValueTask.CompletedTask;
        }

        _value += value.Value;
        return ValueTask.CompletedTask;
    }
}