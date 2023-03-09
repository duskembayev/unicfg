namespace unicfg.Model.Sources;

public interface ISource
{
    StringRef GetText(in Range range);
    SourcePosition GetPosition(in Range range);

    SequenceReader<char> CreateReader();
}