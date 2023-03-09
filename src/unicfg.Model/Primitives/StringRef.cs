using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace unicfg.Model.Primitives;

public readonly struct StringRef : IEquatable<StringRef>, IEquatable<ReadOnlyMemory<char>>, IEquatable<string>
{
    public static readonly StringRef Empty = default;

    private readonly SegmentMemory<char> _memory = SegmentMemory<char>.Empty;

    private StringRef(SegmentMemory<char> memory)
    {
        _memory = memory;
    }

    public bool Equals(StringRef other)
    {
        return _memory.Equals(other._memory);
    }

    public bool Equals(ReadOnlyMemory<char> other)
    {
        return _memory.Equals(other);
    }

    public bool Equals([NotNullWhen(true)] string? other)
    {
        return other is not null && _memory.Equals(other.AsMemory());
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            string typed => Equals(typed),
            StringRef typed => Equals(typed),
            ReadOnlyMemory<char> typed => Equals(typed),
            _ => false
        };
    }

    public override int GetHashCode()
    {
        return _memory.GetHashCode();
    }

    public override string ToString()
    {
        var builder = new StringBuilder(Length);

        foreach (var memory in _memory.Segments)
            builder.Append(memory);

        return builder.ToString();
    }

    public bool IsEmpty => _memory.IsEmpty;

    public int Length => _memory.Length;

    public char this[Index index] => _memory[index];

    public StringRef this[Range range] => new(_memory[range]);

    public StringRef Concat(StringRef other)
    {
        return new StringRef(_memory.Concat(other._memory));
    }

    public static implicit operator StringRef(string? value)
    {
        return string.IsNullOrEmpty(value)
            ? Empty
            : new StringRef(SegmentMemory<char>.From(value.AsMemory()));
    }

    public static implicit operator StringRef(ReadOnlyMemory<char> value)
    {
        return value.IsEmpty
            ? Empty
            : new StringRef(SegmentMemory<char>.From(value));
    }

    public static explicit operator string(StringRef value)
    {
        return value.ToString();
    }

    public static bool operator ==(StringRef left, StringRef right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(StringRef left, StringRef right)
    {
        return !(left == right);
    }

    public static StringRef operator +(StringRef left, StringRef right)
    {
        return left.Concat(right);
    }
}