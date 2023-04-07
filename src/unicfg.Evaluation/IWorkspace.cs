using unicfg.Base.Formatters;
using unicfg.Base.Primitives;
using unicfg.Base.SemanticTree;

namespace unicfg.Evaluation;

public interface IWorkspace
{
    ISet<DocumentOutput> Outputs { get; }

    ISet<IFormatter> Formatters { get; }

    void OpenFrom(string filePath);

    void OverrideProperty(SymbolRef property, StringRef value);

    Task<ImmutableArray<EmitResult>> EmitAsync(CancellationToken cancellationToken);
}