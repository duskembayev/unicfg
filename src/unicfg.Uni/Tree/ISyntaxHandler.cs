namespace unicfg.Uni.Tree;

internal interface ISyntaxHandler
{
    bool CanHandle(in Token token);

    HandleResult Handle(ref TokenIndexer indexer);
}
