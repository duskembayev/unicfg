using unicfg.Lexer.Extensions;
using unicfg.Model;

namespace unicfg.Lexer.Handlers;

internal sealed class SingleCharacterLexerHandler : ILexerHandler
{
    private readonly char _character;
    private readonly TokenType _tokenType;

    public SingleCharacterLexerHandler(char character, TokenType tokenType)
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
        var token = new Token(reader.Position.AsRange(1), _tokenType);
        reader.Advance(1);
        return token;
    }
}