using unicfg.Base.Primitives;
using unicfg.Base.SemanticTree;
using unicfg.Base.SyntaxTree.Values;
using unicfg.Evaluation.Walkers;

namespace unicfg.Evaluation.Outputs;

internal sealed class ValueBuilder : AsyncWalker
{
    private readonly IReadOnlyDictionary<SymbolRef, EmitValue> _dependencies;
    private readonly CancellationToken _cancellationToken;
    private readonly HashSet<SymbolRef> _unresolvedDependencies;

    private bool _hasErrors;
    private StringRef _value;

    public ValueBuilder(IReadOnlyDictionary<SymbolRef, EmitValue> dependencies, CancellationToken cancellationToken)
        : base(cancellationToken)
    {
        _dependencies = dependencies;
        _cancellationToken = cancellationToken;
        _unresolvedDependencies = new HashSet<SymbolRef>();
    }

    public IEnumerable<SymbolRef> UnresolvedDependencies => _unresolvedDependencies;

    public EmitValue? GetResult()
    {
        if (_hasErrors)
            return EmitValue.Error;

        if (_unresolvedDependencies.Count > 0)
            return null;

        return EmitValue.CreateEvaluatedValue(_value);
    }

    public void Reset()
    {
        _unresolvedDependencies.Clear();
        _hasErrors = false;
        _value = StringRef.Empty;
    }

    public override ValueTask Visit(TextValue textValue)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        if (_hasErrors)
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

        if (!_dependencies.TryGetValue(refValue.Property, out var value))
        {
            _unresolvedDependencies.Add(refValue.Property);
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