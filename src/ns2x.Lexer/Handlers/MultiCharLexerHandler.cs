namespace ns2x.Lexer.Handlers;

internal abstract class MultiCharLexerHandler : ILexerHandler
{
    public abstract bool CanHandle(char trigger);

    public Token? Handle(ref SequenceReader<char> reader)
    {
        var start = reader.Position;
        var tokenType = OnHandle(ref reader);

        if (start.Equals(reader.Position))
            return null;

        return new Token(start.AsRange(reader.Position), tokenType);
    }

    protected abstract TokenType OnHandle(ref SequenceReader<char> reader);
}