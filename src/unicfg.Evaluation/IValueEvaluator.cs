using unicfg.Base.SemanticTree;
using unicfg.Base.SyntaxTree.Values;

namespace unicfg.Evaluation;

internal interface IValueEvaluator
{
    ValueTask<EmitValue> EvaluateAsync(IValue value, CancellationToken cancellationToken);
}