using System.Buffers;

namespace ns2x.Model;

public interface ISource
{
    StringRef Text(in Token token);
    StringRef Raw(in Token token);
    SourcePosition GetPosition(in Token token);

    SequenceReader<char> CreateReader();
}