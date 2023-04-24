using unicfg.Evaluation.Walkers;

namespace unicfg.Evaluation;

[ContainerEntry(ServiceLifetime.Scoped, typeof(IScopeEvaluator))]
internal sealed class ScopeEvaluator : IScopeEvaluator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IDiagnostics _diagnostics;

    public ScopeEvaluator(IServiceProvider serviceProvider, IDiagnostics diagnostics)
    {
        _serviceProvider = serviceProvider;
        _diagnostics = diagnostics;
    }

    public async Task<EmitScope> EvaluateAsync(
        SymbolRef scopeRef,
        EvaluationContext context,
        CancellationToken cancellationToken)
    {
        return await ActivatorUtilities
            .CreateInstance<IValueEvaluator>(_serviceProvider, context.Entries, context.Overrides)
            .BuildScopeAsync(scopeRef, context.Entries, context.Defaults, _diagnostics, cancellationToken)
            .ConfigureAwait(false);
    }
}
