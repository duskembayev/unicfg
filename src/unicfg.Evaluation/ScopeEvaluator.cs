using unicfg.Base.Extensions;
using unicfg.Evaluation.Walkers;

namespace unicfg.Evaluation;

[ContainerEntry(ServiceLifetime.Scoped, typeof(IScopeEvaluator))]
internal sealed class ScopeEvaluator : IScopeEvaluator
{
    private readonly IValueEvaluatorFactory _valueEvaluatorFactory;
    private readonly IDiagnostics _diagnostics;

    public ScopeEvaluator(IValueEvaluatorFactory valueEvaluatorFactory, IDiagnostics diagnostics)
    {
        _valueEvaluatorFactory = valueEvaluatorFactory;
        _diagnostics = diagnostics;
    }

    public async Task<EmitScope> EvaluateAsync(
        SymbolRef scopeRef,
        EvaluationContext context,
        CancellationToken cancellationToken)
    {
        var valueEvaluator = _valueEvaluatorFactory.Create(context.Entries, context.Overrides);
        var outputBuilder = new EmitScopeBuilder(valueEvaluator, context.Defaults, _diagnostics, cancellationToken);

        foreach (var entry in context.Entries)
        {
            var symbol = entry.FindSymbol(scopeRef);

            if (symbol is not null)
            {
                await symbol.Accept(outputBuilder).ConfigureAwait(false);
            }
        }

        return outputBuilder.Scope;
    }
}
