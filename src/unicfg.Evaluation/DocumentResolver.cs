using System.Diagnostics;
using unicfg.Uni;

namespace unicfg.Evaluation;

[ContainerEntry(ServiceLifetime.Scoped, typeof(IDocumentResolver))]
internal sealed class DocumentResolver : IDocumentResolver
{
    private readonly IUniDocumentReader _uniDocumentReader;

    public DocumentResolver(IUniDocumentReader uniDocumentReader)
    {
        _uniDocumentReader = uniDocumentReader;
    }

    public Task<Document> LoadFromFileAsync(string filePath, CancellationToken cancellationToken)
    {
        // TODO: detect format by file extension
        throw new NotImplementedException();
    }

    public Task<Document> LoadFromFileAsync(string filePath, DocumentFormat format, CancellationToken cancellationToken)
    {
        Debug.Assert(format == DocumentFormat.Uni);
        return _uniDocumentReader.ReadAsync(filePath, cancellationToken);
    }
}
