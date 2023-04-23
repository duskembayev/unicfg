using unicfg.Uni.Lex;
using unicfg.Uni.Tree;

namespace unicfg.Uni;

[ContainerEntry(ServiceLifetime.Scoped, typeof(IUniDocumentReader))]
internal sealed class UniDocumentReader : IUniDocumentReader
{
    private readonly IDiagnostics _diagnostics;
    private readonly IServiceProvider _serviceProvider;

    public UniDocumentReader(IDiagnostics diagnostics, IServiceProvider serviceProvider)
    {
        _diagnostics = diagnostics;
        _serviceProvider = serviceProvider;
    }

    public async Task<Document> ReadAsync(string path, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(path);

        var source = await Source.FromFileAsync(path, cancellationToken).ConfigureAwait(false);
        var diagnostics = _diagnostics.WithSource(source);

        var tokens = _serviceProvider
            .GetRequiredService<ILexer>()
            .Process(source, diagnostics);

        return ActivatorUtilities
            .CreateInstance<IParser>(_serviceProvider, diagnostics)
            .Execute(source, tokens);
    }
}
