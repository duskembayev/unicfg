using unicfg.Base.SyntaxTree;
using unicfg.Evaluation.Walkers;

namespace unicfg.Evaluation.Extensions;

public static class DocumentExtensions
{
    public static ImmutableHashSet<DocumentOutput> GetOutputs(this Document document)
    {
        var outputCollector = new OutputCollector();
        document.Accept(outputCollector);
        return outputCollector.GetResult();
    }
}