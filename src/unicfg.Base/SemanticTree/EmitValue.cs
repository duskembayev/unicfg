using unicfg.Base.Primitives;
using unicfg.Base.SyntaxTree;

namespace unicfg.Base.SemanticTree;

public sealed class EmitValue
{
    public static readonly EmitValue Error = new(EvaluationState.Error, "<ERROR>");

    private readonly StringRef _value;

    private EmitValue(EvaluationState state, StringRef value)
    {
        State = state;
        _value = value;
    }

    public StringRef Value
    {
        get
        {
            if (State != EvaluationState.Evaluated)
                throw new InvalidOperationException();

            return _value;
        }
    }

    public EvaluationState State { get; }

    public static EmitValue CreateEvaluatedValue(StringRef value)
    {
        return new EmitValue(EvaluationState.Evaluated, value);
    }
}