namespace unicfg.Uni.Lex.Handlers;

internal sealed class WhitespacesLexerHandler : MultiCharLexerHandler
{
    public override bool CanHandle(char trigger)
    {
        return char.IsWhiteSpace(trigger) && !trigger.IsEol();
    }

    protected override TokenType OnHandle(ref SequenceReader<char> reader)
    {
        do
        {
            reader.Advance(1);
        } while (reader.TryPeek(out var c) && char.IsWhiteSpace(c) && !c.IsEol());

        return TokenType.Whitespace;
    }
}
