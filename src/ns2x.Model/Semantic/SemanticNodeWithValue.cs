namespace ns2x.Model.Semantic;

public abstract class SemanticNodeWithValue : ISemanticNodeWithName
{
    private const string ErrorValue = "<ERROR>";
    private string _evaluatedValue = string.Empty;

    protected SemanticNodeWithValue(StringRef name, ImmutableArray<IValue> values)
    {
        Values = values;
        Name = name;
    }

    public StringRef Name { get; }
    public IValue Value => Values[^1];
    public ImmutableArray<IValue> Values { get; }

    public string EvaluatedValue
    {
        get
        {
            return EvaluationState switch
            {
                PropertyEvaluationState.Error => ErrorValue,
                PropertyEvaluationState.Evaluated => _evaluatedValue,
                _ => throw new InvalidOperationException()
            };
        }
    }

    public PropertyEvaluationState EvaluationState { get; private set; }

    public void SetEvaluatedValue(string evaluatedValue)
    {
        if (EvaluationState != PropertyEvaluationState.Unevaluated)
            throw new InvalidOperationException();

        _evaluatedValue = evaluatedValue;
        EvaluationState = PropertyEvaluationState.Evaluated;
    }

    public void SetEvaluationError()
    {
        if (EvaluationState != PropertyEvaluationState.Unevaluated)
            throw new InvalidOperationException();

        EvaluationState = PropertyEvaluationState.Error;
    }

    public abstract void Accept(ISemanticNodeVisitor visitor);
}