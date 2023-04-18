using unicfg.Base.Primitives;

namespace unicfg.Uni.Lex;

public interface ILexerHandler
{
    bool CanHandle(char trigger);

    Token? Handle(ref SequenceReader<char> reader);
}