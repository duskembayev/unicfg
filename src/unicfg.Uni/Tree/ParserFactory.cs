namespace unicfg.Uni.Tree;

[ContainerEntry(ServiceLifetime.Singleton, typeof(IParserFactory))]
internal sealed class ParserFactory : IParserFactory
{
    private readonly ICurrentProcess _process;

    public ParserFactory(ICurrentProcess process)
    {
        _process = process;
    }

    public IParser Create(IDiagnostics diagnostics)
    {
        return new Parser(diagnostics, _process);
    }
}
