using unicfg.Base.Extensions;
using unicfg.Base.Primitives;

namespace unicfg.Uni.Lex.Handlers;

internal sealed class CommentLexerHandler : MultiCharLexerHandler
{
    public override bool CanHandle(char trigger)
    {
        return trigger == '#';
    }

    protected override TokenType OnHandle(ref SequenceReader<char> reader)
    {
        do
        {
            reader.Advance(1);
        } while (reader.TryPeek(out var c) && !c.IsEol());

        return TokenType.Comment;
    }
}
