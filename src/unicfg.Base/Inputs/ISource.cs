using unicfg.Base.Primitives;

namespace unicfg.Base.Inputs;

public interface ISource
{
    string? Location { get; }

    StringRef GetText(in Range range);
    SourcePosition GetPosition(in Range range);

    SequenceReader<char> CreateReader();
}
