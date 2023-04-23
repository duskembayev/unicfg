using unicfg.Uni.Lex.Extensions;

namespace unicfg.Uni.Lex.Handlers;

internal sealed class EscapableCharacterLexerHandler : ILexerHandler
{
    private readonly char _character;
    private readonly TokenType _tokenType;

    public EscapableCharacterLexerHandler(char character, TokenType tokenType)
    {
        _character = character;
        _tokenType = tokenType;
    }

    public bool CanHandle(char trigger)
    {
        return trigger == _character;
    }

    public Token? Handle(ref SequenceReader<char> reader)
    {
        var start = reader.Position;
        var length = 0;

        do
        {
            reader.Advance(1);
            length++;
        } while (reader.TryPeek(out var c) && c == _character);

        var rawRange = start.AsRange(reader.Position);

        return length > 1
            ? new Token(
                TokenType.Expression,
                rawRange,
                start.Next().AsRange(reader.Position))
            : new Token(_tokenType, rawRange);
    }
}
