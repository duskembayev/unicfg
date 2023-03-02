using System.Buffers;
using unicfg.Model.Primitives;

namespace unicfg.Model;

public interface ISource
{
    StringRef GetText(in Range range);
    SourcePosition GetPosition(in Range range);

    SequenceReader<char> CreateReader();
}