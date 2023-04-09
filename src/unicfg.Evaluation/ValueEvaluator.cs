using unicfg.Base.SemanticTree;
using unicfg.Base.SyntaxTree.Values;

namespace unicfg.Evaluation;

internal sealed class ValueEvaluator : IValueEvaluator
{
    private readonly EvaluationContext _evaluationContext;

    public ValueEvaluator(EvaluationContext evaluationContext)
    {
        _evaluationContext = evaluationContext;
    }

    public ValueTask<EmitValue> EvaluateAsync(IValue value, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}