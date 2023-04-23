namespace unicfg.Evaluation;

internal interface IValueEvaluator
{
    ValueTask<EmitValue> EvaluateAsync(IElementWithValue element, CancellationToken cancellationToken);
}
