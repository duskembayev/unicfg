using unicfg.Base.Primitives;

namespace unicfg.Base.SemanticTree;

public sealed class EmitValue
{
    public static readonly EmitValue Error = new(EvaluationState.Error, "<ERROR>");

    private EmitValue(EvaluationState state, StringRef value)
    {
        State = state;
        Value = value;
    }

    public StringRef Value { get; }

    public EvaluationState State { get; }

    public static EmitValue CreateEvaluatedValue(StringRef value)
    {
        return new EmitValue(EvaluationState.Evaluated, value);
    }
}
