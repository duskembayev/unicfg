namespace unicfg.Evaluation;

internal interface IValueEvaluatorFactory
{
    IValueEvaluator Create(ImmutableArray<Document> entries, ImmutableDictionary<SymbolRef, StringRef> overrides);
}
