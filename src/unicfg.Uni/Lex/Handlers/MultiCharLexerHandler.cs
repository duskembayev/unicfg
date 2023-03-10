using unicfg.Base.Primitives;
using unicfg.Uni.Lex.Extensions;

namespace unicfg.Uni.Lex.Handlers;

internal abstract class MultiCharLexerHandler : ILexerHandler
{
    public abstract bool CanHandle(char trigger);

    public Token? Handle(ref SequenceReader<char> reader)
    {
        var start = reader.Position;
        var tokenType = OnHandle(ref reader);

        if (start.Equals(reader.Position))
            return null;

        return new Token(tokenType, start.AsRange(reader.Position));
    }

    protected abstract TokenType OnHandle(ref SequenceReader<char> reader);
}