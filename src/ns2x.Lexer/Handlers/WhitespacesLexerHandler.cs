namespace ns2x.Lexer.Handlers;

internal sealed class WhitespacesLexerHandler : ILexerHandler
{
    public bool CanHandle(char trigger)
    {
        return char.IsWhiteSpace(trigger) && !trigger.IsEol();
    }

    public Token? Handle(ref SequenceReader<char> reader)
    {
        do
        {
            reader.Advance(1);
        } while (reader.TryPeek(out var c) && char.IsWhiteSpace(c));

        return null;
    }
}