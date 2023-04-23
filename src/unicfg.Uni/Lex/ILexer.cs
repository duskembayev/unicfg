namespace unicfg.Uni.Lex;

internal interface ILexer
{
    ImmutableArray<Token> Process(ISource source, IDiagnostics diagnostics);
}
