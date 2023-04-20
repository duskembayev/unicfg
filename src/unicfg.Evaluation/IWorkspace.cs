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

    void DefaultPropertyValue(SymbolRef propertyPath, StringRef value);

    void OverridePropertyValue(SymbolRef propertyPath, StringRef value);

    Task<ImmutableArray<EmitResult>> EmitAsync(CancellationToken cancellationToken);
}