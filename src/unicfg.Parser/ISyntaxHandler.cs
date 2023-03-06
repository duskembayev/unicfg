using unicfg.Model;

namespace unicfg.Parser;

internal interface ISyntaxHandler
{
    bool CanHandle(in Token token);

    HandleResult Handle(ref TokenIndexer indexer);
}