using unicfg.Base.SemanticTree;
using unicfg.Base.SyntaxTree;

namespace unicfg.Evaluation.Outputs;

internal interface IValueEvaluator
{
    ValueTask<EmitValue> EvaluateAsync(IElementWithValue element, CancellationToken cancellationToken);
}
