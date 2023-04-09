using unicfg.Base.Formatters;
using unicfg.Base.Primitives;
using unicfg.Base.SemanticTree;
using unicfg.Base.SyntaxTree;

namespace unicfg.Evaluation;

public interface IWorkspace
{
    ISet<DocumentOutput> Outputs { get; }

    ISet<IFormatter> Formatters { get; }

    Task OpenFromAsync(string filePath, CancellationToken cancellationToken);
    Task OpenAsync(Document document, CancellationToken cancellationToken);

    void OverrideProperty(SymbolRef property, StringRef value);

    Task<ImmutableArray<EmitResult>> EmitAsync(CancellationToken cancellationToken);
}