namespace unicfg.Uni.Tree;

internal interface IParser
{
    Document Execute(ISource source, ImmutableArray<Token> tokens);
}
