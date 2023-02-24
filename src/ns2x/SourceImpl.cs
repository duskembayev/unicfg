namespace ns2x;

public sealed class SourceImpl : ISource
{
    private readonly ReadOnlyMemory<char> _memory;

    public SourceImpl(ReadOnlyMemory<char> memory)
    {
        _memory = memory;
    }

    public StringRef Text(in Token token)
    {
        var raw = _memory[token.Range];
        return token.Type == TokenType.QuotedExpression ? raw[1..^1] : raw;
    }

    public StringRef Raw(in Token token)
    {
        return _memory[token.Range];
    }

    public SequenceReader<char> CreateReader()
    {
        return new SequenceReader<char>(new ReadOnlySequence<char>(_memory));
    }
}