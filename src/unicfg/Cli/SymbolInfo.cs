namespace unicfg.Cli;

internal record SymbolInfo(string Path)
{
    public static readonly SymbolInfo Root = new(string.Empty);
}