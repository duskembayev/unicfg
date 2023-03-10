using unicfg.Base.Primitives;

namespace unicfg.Base.Sources;

public interface ISource
{
    string? Location { get; }

    StringRef GetText(in Range range);
    SourcePosition GetPosition(in Range range);

    SequenceReader<char> CreateReader();
}