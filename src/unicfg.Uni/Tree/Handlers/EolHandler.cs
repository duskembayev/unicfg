using unicfg.Base.Primitives;

namespace unicfg.Uni.Tree.Handlers;

internal sealed class EolHandler : ISyntaxHandler
{
    public bool CanHandle(in Token token)
    {
        return token.IsEndOfLine();
    }

    public HandleResult Handle(ref TokenIndexer indexer)
    {
        indexer = indexer.Next;
        return HandleResult.SuccessResult;
    }
}
