using unicfg.Lexer.Extensions;
using unicfg.Model;

namespace unicfg.Lexer.Handlers;

internal sealed class QuotedExpressionLexerHandler : MultiCharLexerHandler
{
    private readonly char _quote;

    public QuotedExpressionLexerHandler(char quote)
    {
        _quote = quote;
    }

    public override bool CanHandle(char trigger)
    {
        return trigger == _quote;
    }

    protected override TokenType OnHandle(ref SequenceReader<char> reader)
    {
        reader.Advance(1);

        while (reader.TryRead(out var c))
        {
            if (c == _quote)
                return TokenType.QuotedExpression;

            if (c.IsEol())
            {
                reader.Rewind(1);
                break;
            }
        }

        return TokenType.Unknown;
    }
}