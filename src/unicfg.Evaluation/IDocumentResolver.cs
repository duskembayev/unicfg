using System.Text;
using unicfg.Base.SyntaxTree;

namespace unicfg.Evaluation;

public interface IDocumentResolver
{
    Task<Document> LoadFromFileAsync(string filePath, CancellationToken cancellationToken);
    Task<Document> LoadFromFileAsync(string filePath, DocumentFormat format, CancellationToken cancellationToken);
}