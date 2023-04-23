namespace unicfg.Evaluation;

internal interface IScopeEvaluator
{
    Task<EmitScope> EvaluateAsync(
        SymbolRef scopeRef,
        EvaluationContext context,
        CancellationToken cancellationToken);
}
