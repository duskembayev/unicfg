using unicfg.Base.Formatters;
using unicfg.Base.Primitives;
using unicfg.Base.SemanticTree;
using unicfg.Base.SyntaxTree;

namespace unicfg.Evaluation;

public sealed class EvaluationFormatter : IFormatter
{
    private readonly TextWriter _writer;

    public EvaluationFormatter(TextWriter writer, IElementEvaluator evaluator)
    {
        _writer = writer;
    }

    public bool Matches(IReadOnlyDictionary<StringRef, StringRef> attributes) => true;

    public Task<EmitResult> FormatAsync(EmitScope scope, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}