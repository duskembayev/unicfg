using unicfg.Base.Primitives;

namespace unicfg.Evaluation.EmitModel;

public record EmitResult(
    SymbolRef Scope,
    string OutputPath,
    int TotalPropertyCount,
    int ErrorPropertyCount)
{
    public bool HasErrors => ErrorPropertyCount > 0;
}