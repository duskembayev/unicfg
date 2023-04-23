namespace unicfg.Base.Primitives;

internal readonly struct SegmentMemory<T> : IEquatable<ReadOnlyMemory<T>>, IEquatable<SegmentMemory<T>>
    where T : struct
{
    public static readonly SegmentMemory<T> Empty = default;

    private readonly ImmutableArray<ReadOnlyMemory<T>> _segments = ImmutableArray<ReadOnlyMemory<T>>.Empty;

    private SegmentMemory(ImmutableArray<ReadOnlyMemory<T>> segments)
    {
        _segments = segments;
        Length = GetTotalLength(_segments);
    }

    public int Length { get; } = 0;

    public bool IsEmpty => Length == 0;
    public ImmutableArray<ReadOnlyMemory<T>> Segments => _segments;

    public T this[Index index]
    {
        get
        {
            var offset = index.GetOffset(Length);

            if (offset >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var k = 0;

            while (k < _segments.Length && offset >= _segments[k].Length)
            {
                offset -= _segments[k++].Length;
            }

            return _segments[k].Span[offset];
        }
    }

    public SegmentMemory<T> this[Range range]
    {
        get
        {
            var start = range.Start.GetOffset(Length);
            var end = range.End.GetOffset(Length);

            if (start >= Length || end >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(range));
            }

            if (start == end)
            {
                return Empty;
            }

            if (start > end)
            {
                throw new InvalidOperationException();
            }

            var builder = ImmutableArray.CreateBuilder<ReadOnlyMemory<T>>(_segments.Length);

            for (var k = 0; k < _segments.Length; k++)
            {
                if (start >= _segments[k].Length && end >= _segments[k].Length)
                {
                    start -= _segments[k].Length;
                    end -= _segments[k].Length;
                    continue;
                }

                if (start < _segments[k].Length && end >= _segments[k].Length)
                {
                    builder.Add(_segments[k][start..]);
                    start = 0;
                    end -= _segments[k].Length;
                    continue;
                }

                builder.Add(_segments[k][start..end]);
                break;
            }

            return new SegmentMemory<T>(builder.ToImmutable());
        }
    }

    public bool Equals(ReadOnlyMemory<T> other)
    {
        return Equals(From(other));
    }

    public bool Equals(SegmentMemory<T> other)
    {
        if (Length != other.Length)
        {
            return false;
        }

        if (Length == 0)
        {
            return true;
        }

        var k = 0;
        var kOffset = 0;
        var j = 0;
        var jOffset = 0;

        while (k < _segments.Length)
        {
            if (!_segments[k].Span[kOffset++].Equals(other._segments[j].Span[jOffset++]))
            {
                return false;
            }

            if (kOffset >= _segments[k].Length)
            {
                k++;
                kOffset = 0;
            }

            if (jOffset >= other._segments[j].Length)
            {
                j++;
                jOffset = 0;
            }
        }

        return true;
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            SegmentMemory<T> typed => Equals(typed),
            ReadOnlyMemory<T> typed => Equals(typed),
            _ => false
        };
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        var k = 0;
        var kOffset = 0;

        while (!IsEmpty && k < _segments.Length)
        {
            hashCode.Add(_segments[k].Span[kOffset++]);

            if (kOffset >= _segments[k].Length)
            {
                k++;
                kOffset = 0;
            }
        }

        hashCode.Add(Length);
        return hashCode.ToHashCode();
    }

    public SegmentMemory<T> Concat(SegmentMemory<T> other)
    {
        if (other.IsEmpty)
        {
            return this;
        }

        if (IsEmpty)
        {
            return other;
        }

        return new SegmentMemory<T>(_segments.AddRange(other._segments));
    }

    public SegmentMemory<T> Concat(ReadOnlyMemory<T> other)
    {
        if (other.IsEmpty)
        {
            return this;
        }

        if (IsEmpty)
        {
            return From(other);
        }

        return new SegmentMemory<T>(_segments.Add(other));
    }

    public static SegmentMemory<T> From(ReadOnlyMemory<T> memory)
    {
        if (memory.IsEmpty)
        {
            return Empty;
        }

        return new SegmentMemory<T>(ImmutableArray.Create(memory));
    }

    private static int GetTotalLength(ImmutableArray<ReadOnlyMemory<T>> array)
    {
        if (array.IsDefaultOrEmpty)
        {
            return 0;
        }

        var result = 0;

        for (var k = 0; k < array.Length; k++)
        {
            result += array[k].Length;
        }

        return result;
    }
}
