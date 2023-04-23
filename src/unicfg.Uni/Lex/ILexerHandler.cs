namespace unicfg.Uni.Lex;

internal interface ILexerHandler
{
    bool CanHandle(char trigger);

    Token? Handle(ref SequenceReader<char> reader);
}
