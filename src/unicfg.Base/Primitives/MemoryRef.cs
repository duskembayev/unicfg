namespace unicfg.Base.Primitives;

/// <summary>
/// Represents a memory segment that is capable of holding a reference to multiple memory segments.
/// Supports concatenation of multiple memory segments.
/// </summary>
/// <typeparam name="T"> The object type from which the contiguous region of memory will be read.</typeparam>
public readonly struct MemoryRef<T> : IEquatable<ReadOnlyMemory<T>>, IEquatable<MemoryRef<T>>
    where T : struct
{
    /// <summary>
    /// The empty <see cref="MemoryRef{T}"/> instance.
    /// Can be used to avoid unnecessary allocations.
    /// </summary>
    public static readonly MemoryRef<T> Empty = default;

    private readonly ImmutableArray<ReadOnlyMemory<T>> _segments = ImmutableArray<ReadOnlyMemory<T>>.Empty;

    private MemoryRef(ImmutableArray<ReadOnlyMemory<T>> segments)
    {
        _segments = segments;
        Length = GetTotalLength(_segments);
    }

    /// <summary>
    /// The length of the <see cref="MemoryRef{T}"/>.
    /// </summary>
    public int Length { get; } = 0;

    /// <summary>
    /// Flag indicating whether the <see cref="MemoryRef{T}"/> is empty.
    /// </summary>
    public bool IsEmpty => Length == 0;
    
    /// <summary>
    /// Array of memory segments that this <see cref="MemoryRef{T}"/> is composed of.
    /// </summary>
    public ImmutableArray<ReadOnlyMemory<T>> Segments => _segments;

    /// <summary>
    /// Returns the specified element of the <see cref="MemoryRef{T}"/>.
    /// </summary>
    /// <param name="index"></param>
    /// <exception cref="System.IndexOutOfRangeException">
    /// Thrown when index less than 0 or index greater than or equal to Length
    /// </exception>
    public T this[Index index]
    {
        get
        {
            var offset = index.GetOffset(Length);

            if (offset >= Length)
            {
                throw new IndexOutOfRangeException("Index was outside the bounds of the SegmentMemory.");
            }

            var k = 0;

            while (k < _segments.Length && offset >= _segments[k].Length)
            {
                offset -= _segments[k++].Length;
            }

            return _segments[k].Span[offset];
        }
    }

    /// <summary>
    /// Returns a new <see cref="MemoryRef{T}"/> that represents a portion of the current instance.
    /// </summary>
    /// <param name="range"></param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when start or end of the range is outside the bounds of the SegmentMemory.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when start of the range is greater than end of the range.
    /// </exception>
    public MemoryRef<T> this[Range range]
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

            return new MemoryRef<T>(builder.ToImmutable());
        }
    }

    /// <summary>
    /// Compares the current instance with the specified <see cref="ReadOnlyMemory{T}"/> for equality.
    /// </summary>
    /// <param name="other"></param>
    public bool Equals(ReadOnlyMemory<T> other)
    {
        return Equals(From(other));
    }

    /// <summary>
    /// Compares the current instance with the specified <see cref="MemoryRef{T}"/> for equality.
    /// </summary>
    /// <param name="other"></param>
    public bool Equals(MemoryRef<T> other)
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

    /// <summary>
    /// Compares the current instance with the specified <see cref="object"/> for equality.
    /// </summary>
    /// <param name="obj"></param>
    public override bool Equals(object? obj)
    {
        return obj switch
        {
            MemoryRef<T> typed => Equals(typed),
            ReadOnlyMemory<T> typed => Equals(typed),
            _ => false
        };
    }

    /// <summary>
    /// Returns the hash code for the current instance.
    /// </summary>
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

    /// <summary>
    /// Returns a new <see cref="MemoryRef{T}"/> that is the result of concatenating the current instance with the specified <see cref="MemoryRef{T}"/>.
    /// </summary>
    public MemoryRef<T> Concat(MemoryRef<T> other)
    {
        if (other.IsEmpty)
        {
            return this;
        }

        if (IsEmpty)
        {
            return other;
        }

        return new MemoryRef<T>(_segments.AddRange(other._segments));
    }

    /// <summary>
    /// Returns a new <see cref="MemoryRef{T}"/> that is the result of concatenating the current instance with the specified <see cref="ReadOnlyMemory{T}"/>.
    /// </summary>
    public MemoryRef<T> Concat(ReadOnlyMemory<T> other)
    {
        if (other.IsEmpty)
        {
            return this;
        }

        if (IsEmpty)
        {
            return From(other);
        }

        return new MemoryRef<T>(_segments.Add(other));
    }

    /// <summary>
    /// Creates a new <see cref="MemoryRef{T}"/> from the specified <see cref="ReadOnlyMemory{T}"/>.
    /// </summary>
    /// <param name="memory"></param>
    /// <returns></returns>
    public static MemoryRef<T> From(ReadOnlyMemory<T> memory)
    {
        if (memory.IsEmpty)
        {
            return Empty;
        }

        return new MemoryRef<T>(ImmutableArray.Create(memory));
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
