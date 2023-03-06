using unicfg.Model;
using unicfg.Model.Primitives;

namespace unicfg.Parser.Extensions;

internal static class TokenIndexerExtensions
{
    public static bool SkipWhitespace(this ref TokenIndexer indexer)
    {
        return indexer.MoveTo(type => type != TokenType.Whitespace);
    }
    
    public static bool MoveTo(this ref TokenIndexer indexer, Predicate<TokenType> predicate)
    {
        while (!indexer.OutOfRange && !predicate.Invoke(indexer.Token.Type))
            indexer = indexer.Next;

        return !indexer.OutOfRange;
    }

    public static StringRef ReadContentTo(this ref TokenIndexer indexer, Predicate<TokenType> predicate)
    {
        var result = StringRef.Empty;

        while (!indexer.OutOfRange && !predicate.Invoke(indexer.Token.Type))
        {
            result = result.Concat(indexer.Content);
            indexer = indexer.Next;
        }

        return result;
    }
    
    public static bool TryReadContentTo(this ref TokenIndexer indexer, Predicate<TokenType> predicate, out StringRef result)
    {
        result = indexer.ReadContentTo(predicate);
        return !result.IsEmpty;
    }

    public static void NextNotWhitespace(this ref TokenIndexer indexer)
    {
        indexer = indexer.Next;
        indexer.SkipWhitespace();
    }
}