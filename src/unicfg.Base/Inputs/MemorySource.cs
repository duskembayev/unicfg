﻿using unicfg.Base.Extensions;
using unicfg.Base.Primitives;

namespace unicfg.Base.Inputs;

internal sealed class MemorySource : ISource
{
    private readonly ImmutableArray<int> _lines;
    private readonly ReadOnlyMemory<char> _memory;

    public MemorySource(ReadOnlyMemory<char> memory)
    {
        _memory = memory;
        _lines = DetermineLines(_memory.Span);
    }

    public string? Location { get; init; }

    public StringRef GetText(in Range range)
    {
        return _memory[range];
    }

    public SourcePosition GetPosition(in Range range)
    {
        var (startLine, startColumn) = GetPosition(range.Start);
        var (endLine, endColumn) = GetPosition(range.End);

        return new SourcePosition(startLine, startColumn, endLine, endColumn);
    }

    public SequenceReader<char> CreateReader()
    {
        return new SequenceReader<char>(new ReadOnlySequence<char>(_memory));
    }

    private (int line, int column) GetPosition(Index index)
    {
        if (_lines.Length == 0)
        {
            return (0, 0);
        }

        var offset = index.GetOffset(_memory.Length);
        var line = _lines.BinarySearch(offset);

        if (line >= 0)
        {
            return (line + 1, 1);
        }

        line = ~line;
        return (line, offset - _lines[line - 1] + 1);
    }

    private static ImmutableArray<int> DetermineLines(ReadOnlySpan<char> text)
    {
        if (text.Length == 0)
        {
            return ImmutableArray<int>.Empty;
        }

        var lines = ImmutableArray.CreateBuilder<int>();
        var unread = text;
        var offset = 0;

        do
        {
            lines.Add(offset);

            var eol = unread.IndexOfEol(out var stride);

            if (eol < 0)
            {
                break;
            }

            offset += eol + stride;
            unread = unread[(eol + stride)..];
        } while (unread.Length > 0);

        return lines.ToImmutable();
    }
}
