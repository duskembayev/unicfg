using unicfg.Model.Elements.Values;

namespace unicfg.Model.Elements;

public abstract class ElementWithValue : ElementWithName
{
    private static readonly StringRef ErrorValue = "<ERROR>";
    private StringRef _evaluatedValue = StringRef.Empty;

    protected ElementWithValue(StringRef name, Document document, ElementWithName parent)
        : base(name, document, parent)
    {
    }

    public IValue Value => Values[^1];
    public ImmutableArray<IValue> Values { get; internal set; }

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