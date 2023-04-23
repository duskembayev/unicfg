namespace unicfg.Uni;

public interface IUniDocumentReader
{
    Task<Document> ReadAsync(string path, CancellationToken cancellationToken);
}
