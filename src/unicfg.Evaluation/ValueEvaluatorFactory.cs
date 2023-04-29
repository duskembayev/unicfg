namespace unicfg.Evaluation;

[ContainerEntry(ServiceLifetime.Scoped, typeof(IValueEvaluatorFactory))]
internal sealed class ValueEvaluatorFactory : IValueEvaluatorFactory
{
    private readonly IDiagnostics _diagnostics;
    private readonly ILoggerFactory _loggerFactory;

    public ValueEvaluatorFactory(IDiagnostics diagnostics, ILoggerFactory loggerFactory)
    {
        _diagnostics = diagnostics;
        _loggerFactory = loggerFactory;
    }

    public IValueEvaluator Create(ImmutableArray<Document> entries, ImmutableDictionary<SymbolRef, StringRef> overrides)
    {
        return new ValueEvaluator(entries, overrides, _diagnostics, _loggerFactory.CreateLogger<ValueEvaluator>());
    }
}
