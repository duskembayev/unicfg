namespace unicfg.Uni.Tree;

internal interface IParserFactory
{
    IParser Create(IDiagnostics diagnostics);
}
