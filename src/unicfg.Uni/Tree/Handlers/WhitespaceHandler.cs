namespace unicfg.Uni.Tree.Handlers;

internal sealed class WhitespaceHandler : ISyntaxHandler
{
    public bool CanHandle(in Token token)
    {
        return token.IsWhitespace();
    }

    public HandleResult Handle(ref TokenIndexer indexer)
    {
        indexer = indexer.Next;
        return HandleResult.SuccessResult;
    }
}