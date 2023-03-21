using unicfg.Base.Elements;

namespace unicfg.Evaluation;

public interface IDocumentResolver
{
    Document LoadFromFile(string filePath);
    Document LoadFromFile(string filePath, DocumentFormat format);
    Document LoadFromEnvironment();
}