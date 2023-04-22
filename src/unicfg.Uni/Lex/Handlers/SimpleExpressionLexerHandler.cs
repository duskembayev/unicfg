using unicfg.Base.Primitives;

namespace unicfg.Uni.Lex.Handlers;

internal sealed class SimpleExpressionLexerHandler : MultiCharLexerHandler
{
    private const string ExpressionChars = "_-+*\\/|,;:<>`&^%?";

    public override bool CanHandle(char trigger)
    {
        return char.IsLetterOrDigit(trigger) || ExpressionChars.Contains(trigger);
    }

    protected override TokenType OnHandle(ref SequenceReader<char> reader)
    {
        do
        {
            reader.Advance(1);
        } while (reader.TryPeek(out var c) && (char.IsLetterOrDigit(c) || ExpressionChars.Contains(c)));

        return TokenType.Expression;
    }
}
