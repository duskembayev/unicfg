namespace ns2x.Lexer;

public interface ILexerHandler
{
    bool CanHandle(char trigger);

    Token? Handle(ref SequenceReader<char> reader);
}