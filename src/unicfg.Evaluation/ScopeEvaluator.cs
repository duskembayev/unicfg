using unicfg.Evaluation.Walkers;

namespace unicfg.Evaluation;

[ContainerEntry(ServiceLifetime.Scoped, typeof(IScopeEvaluator))]
internal sealed class ScopeEvaluator : IScopeEvaluator
{
    private readonly IServiceProvider _serviceProvider;

    public ScopeEvaluator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<EmitScope> EvaluateAsync(
        SymbolRef scopeRef,
        EvaluationContext context,
        CancellationToken cancellationToken)
    {
        return await ActivatorUtilities
            .CreateInstance<IValueEvaluator>(_serviceProvider, context.Entries, context.Overrides)
            .BuildScopeAsync(scopeRef, context.Entries, context.Defaults, cancellationToken)
            .ConfigureAwait(false);
    }
}
