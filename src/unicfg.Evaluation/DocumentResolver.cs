using System.Diagnostics;
using unicfg.Base.Analysis;
using unicfg.Base.Environment;
using unicfg.Base.Inputs;
using unicfg.Base.SyntaxTree;
using unicfg.Uni.Lex;
using unicfg.Uni.Tree;

namespace unicfg.Evaluation;

[ContainerEntry(ServiceLifetime.Scoped, typeof(IDocumentResolver))]
internal sealed class DocumentResolver : IDocumentResolver
{
    private readonly ICurrentProcess _currentProcess;
    private readonly Diagnostics _diagnostics;

    public DocumentResolver(Diagnostics diagnostics, ICurrentProcess currentProcess)
    {
        _diagnostics = diagnostics;
        _currentProcess = currentProcess;
    }

    public Task<Document> LoadFromFileAsync(string filePath, CancellationToken cancellationToken)
    {
        // TODO: detect format by file extension
        throw new NotImplementedException();
    }

    public Task<Document> LoadFromFileAsync(string filePath, DocumentFormat format, CancellationToken cancellationToken)
    {
        Debug.Assert(format == DocumentFormat.Uni);

        var source = Source.FromFile(filePath);
        var diagnostics = _diagnostics.WithSource(source);
        var tokens = new LexerImpl(diagnostics).Process(source);
        var document = new ParserImpl(diagnostics, _currentProcess).Execute(source, tokens);

        return Task.FromResult(document);
    }
}
