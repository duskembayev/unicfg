using unicfg.Lexer.Extensions;
using unicfg.Model;

namespace unicfg.Lexer.Handlers;

internal sealed class EolLexerHandler : MultiCharLexerHandler
{
    public override bool CanHandle(char trigger)
    {
        return trigger.IsEol();
    }

    protected override TokenType OnHandle(ref SequenceReader<char> reader)
    {
        do
        {
            reader.Advance(1);
        } while (reader.TryPeek(out var c) && c.IsEol());

        return TokenType.Eol;
    }
}