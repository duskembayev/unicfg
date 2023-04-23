namespace unicfg.Evaluation;

internal interface IDocumentResolver
{
    Task<Document> LoadFromFileAsync(string filePath, CancellationToken cancellationToken);
    Task<Document> LoadFromFileAsync(string filePath, DocumentFormat format, CancellationToken cancellationToken);
}
