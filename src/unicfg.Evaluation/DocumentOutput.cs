namespace unicfg.Evaluation;

public sealed class DocumentOutput
{
    public DocumentOutput(SymbolRef scopeRef)
    {
        ScopeRef = scopeRef;
    }

    public SymbolRef ScopeRef { get; }
}
