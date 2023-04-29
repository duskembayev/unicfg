using unicfg.Uni.Lex;
using unicfg.Uni.Tree;

namespace unicfg.Uni;

[ContainerEntry(ServiceLifetime.Scoped, typeof(IUniDocumentReader))]
internal sealed class UniDocumentReader : IUniDocumentReader
{
    private readonly IDiagnostics _diagnostics;
    private readonly IServiceProvider _serviceProvider;
    private readonly IParserFactory _parserFactory;

    public UniDocumentReader(IDiagnostics diagnostics, IServiceProvider serviceProvider, IParserFactory parserFactory)
    {
        _diagnostics = diagnostics;
        _serviceProvider = serviceProvider;
        _parserFactory = parserFactory;
    }

    public async Task<Document> ReadAsync(string path, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(path);

        var source = await Source.FromFileAsync(path, cancellationToken).ConfigureAwait(false);
        var diagnostics = _diagnostics.WithSource(source);

        var tokens = _serviceProvider
            .GetRequiredService<ILexer>()
            .Process(source, diagnostics);

        return _parserFactory.Create(diagnostics).Execute(source, tokens);
    }
}
