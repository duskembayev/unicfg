using unicfg.Base.Primitives;
using unicfg.Base.SyntaxTree;

namespace unicfg.Base.SemanticTree;

public sealed class EmitValue
{
    private static readonly StringRef ErrorValue = "<ERROR>";
    private StringRef _evaluatedValue = StringRef.Empty;

    public StringRef EvaluatedValue
    {
        get
        {
            return EvaluationState switch
            {
                EvaluationState.Error => ErrorValue,
                EvaluationState.Evaluated => _evaluatedValue,
                _ => throw new InvalidOperationException()
            };
        }
    }

    public EvaluationState EvaluationState { get; private set; }

    public void SetEvaluatedValue(StringRef evaluatedValue)
    {
        if (EvaluationState != EvaluationState.Unevaluated)
            throw new InvalidOperationException();

        _evaluatedValue = evaluatedValue;
        EvaluationState = EvaluationState.Evaluated;
    }

    public void SetEvaluationError()
    {
        if (EvaluationState != EvaluationState.Unevaluated)
            throw new InvalidOperationException();

        EvaluationState = EvaluationState.Error;
    }
}