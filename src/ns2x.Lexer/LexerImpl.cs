using ns2x.Lexer.Handlers;
using ns2x.Model.Analysis;

namespace ns2x.Lexer;

public sealed class LexerImpl
{
    private readonly Diagnostics _diagnostics;

    private readonly ImmutableArray<ILexerHandler> _handlers = ImmutableArray.Create<ILexerHandler>(
        new WhitespacesLexerHandler(),
        new EolLexerHandler(),
        new CommentLexerHandler(),
        new SimpleExpressionLexerHandler(),
        new QuotedExpressionLexerHandler('\''),
        new QuotedExpressionLexerHandler('\"'),
        new SingleCharacterLexerHandler('.', TokenType.Dot),
        new SingleCharacterLexerHandler('=', TokenType.Equality),
        new SingleCharacterLexerHandler('$', TokenType.Ref),
        new SingleCharacterLexerHandler('{', TokenType.BraceL),
        new SingleCharacterLexerHandler('}', TokenType.BraceR),
        new SingleCharacterLexerHandler('[', TokenType.BracketL),
        new SingleCharacterLexerHandler(']', TokenType.BracketR)
    );

    public LexerImpl(Diagnostics diagnostics)
    {
        _diagnostics = diagnostics;
    }

    public ImmutableArray<Token> Process(ISource source)
    {
        var sourceReader = source.CreateReader();
        var result = ImmutableArray.CreateBuilder<Token>();

        while (sourceReader.TryPeek(out var c))
        {
            if (!TryHandle(ref sourceReader, c, out var currentToken))
            {
                currentToken = new Token(sourceReader.Position.AsRange(1), TokenType.Unknown);
                sourceReader.Advance(1);
            }

            if (currentToken.HasValue)
            {
                result.Add(currentToken.Value);
                
                if (currentToken is { Type: TokenType.Unknown })
                    _diagnostics.Report(DiagnosticDescriptor.UnknownToken, currentToken.Value.Range);
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
            if (!_handlers[index].CanHandle(c)) continue;
            token = _handlers[index].Handle(ref inputReader);
            return true;
        }

        return false;
    }
}