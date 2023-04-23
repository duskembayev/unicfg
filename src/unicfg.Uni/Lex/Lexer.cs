using unicfg.Uni.Lex.Extensions;
using unicfg.Uni.Lex.Handlers;

namespace unicfg.Uni.Lex;

[ContainerEntry(ServiceLifetime.Transient, typeof(ILexer))]
internal sealed class Lexer : ILexer
{
    private readonly ImmutableArray<ILexerHandler> _handlers = ImmutableArray.Create<ILexerHandler>(
        new WhitespacesLexerHandler(),
        new EolLexerHandler(),
        new CommentLexerHandler(),
        new SimpleExpressionLexerHandler(),
        new EscapableCharacterLexerHandler('.', TokenType.Dot),
        new EscapableCharacterLexerHandler('=', TokenType.Equality),
        new EscapableCharacterLexerHandler('$', TokenType.Ref),
        new EscapableCharacterLexerHandler('{', TokenType.BraceL),
        new EscapableCharacterLexerHandler('}', TokenType.BraceR),
        new EscapableCharacterLexerHandler('[', TokenType.BracketL),
        new EscapableCharacterLexerHandler(']', TokenType.BracketR),
        new EscapableCharacterLexerHandler('@', TokenType.Reserved),
        new EscapableCharacterLexerHandler('!', TokenType.Reserved),
        new EscapableCharacterLexerHandler('(', TokenType.Reserved),
        new EscapableCharacterLexerHandler(')', TokenType.Reserved),
        new EscapableCharacterLexerHandler('\"', TokenType.Reserved),
        new EscapableCharacterLexerHandler('\'', TokenType.Reserved));

    public ImmutableArray<Token> Process(ISource source, IDiagnostics diagnostics)
    {
        var sourceReader = source.CreateReader();
        var result = ImmutableArray.CreateBuilder<Token>();

        while (sourceReader.TryPeek(out var c))
        {
            if (!TryHandle(ref sourceReader, c, out var currentToken))
            {
                currentToken = new Token(TokenType.Unknown, sourceReader.Position.AsRange(1));
                sourceReader.Advance(1);
            }

            if (currentToken.HasValue)
            {
                result.Add(currentToken.Value);

                if (currentToken is { Type: TokenType.Unknown })
                {
                    diagnostics.Report(UnknownToken, source, currentToken.Value.RawRange);
                }
            }
        }

        result.Add(Token.Eof);
        return result.ToImmutable();
    }

    private bool TryHandle(ref SequenceReader<char> inputReader, char c, out Token? token)
    {
        token = null;

        for (var index = 0; index < _handlers.Length; index++)
        {
            if (!_handlers[index].CanHandle(c))
            {
                continue;
            }

            token = _handlers[index].Handle(ref inputReader);
            return true;
        }

        return false;
    }
}
