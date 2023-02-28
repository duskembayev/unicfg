using System.Buffers;

namespace ns2x.Model;

public interface ISource
{
    StringRef GetText(in Range range);
    SourcePosition GetPosition(in Range range);

    SequenceReader<char> CreateReader();
}