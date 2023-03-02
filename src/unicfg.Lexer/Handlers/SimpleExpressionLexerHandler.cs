using unicfg.Model;

namespace unicfg.Lexer.Handlers;

internal sealed class SimpleExpressionLexerHandler : MultiCharLexerHandler
{
    public override bool CanHandle(char trigger)
    {
        return char.IsLetterOrDigit(trigger);
    }

    protected override TokenType OnHandle(ref SequenceReader<char> reader)
    {
        do
        {
            reader.Advance(1);
        } while (reader.TryPeek(out var c) && char.IsLetterOrDigit(c));

        return TokenType.Expression;
    }
}