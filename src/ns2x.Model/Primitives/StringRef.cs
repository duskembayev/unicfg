using System.Diagnostics.CodeAnalysis;

namespace ns2x.Model.Primitives;

public readonly struct StringRef : IEquatable<StringRef>, IEquatable<string>, IEquatable<ReadOnlyMemory<char>>
{
    public static readonly StringRef Empty = default;

    private readonly ReadOnlyMemory<char> _memory = ReadOnlyMemory<char>.Empty;

    private StringRef(ReadOnlyMemory<char> memory)
    {
        _memory = memory;
    }

    public bool Equals(StringRef other)
    {
        return Equals(other._memory);
    }

    public bool Equals([NotNullWhen(true)] string? other)
    {
        return other is not null && Equals(new StringRef(other.AsMemory()));
    }

    public bool Equals(ReadOnlyMemory<char> other)
    {
        if (other.Length != _memory.Length)
            return false;

        var index = -1;
        var result = true;

        while (result && ++index < other.Length)
            result &= other.Span[index].Equals(this[index]);

        return result;
    }

    public override bool Equals(object? obj)
    {
        return obj switch
        {
            string @string => Equals(@string),
            StringRef @ref => Equals(@ref),
            ReadOnlyMemory<char> memory => Equals(memory),
            _ => false
        };
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        for (var index = 0; index < _memory.Span.Length; index++)
            hashCode.Add(_memory.Span[index]);

        return hashCode.ToHashCode();
    }

    public override string ToString()
    {
        if (IsEmpty)
            return string.Empty;

        return new string(_memory.Span);
    }

    public ReadOnlySpan<char>.Enumerator GetEnumerator()
    {
        return _memory.Span.GetEnumerator();
    }

    public bool IsEmpty => _memory.IsEmpty;

    public int Length => _memory.Span.Length;

    public char this[int index] => _memory.Span[index];

    public static implicit operator StringRef(string? value)
    {
        return string.IsNullOrEmpty(value) ? Empty : new StringRef(value.AsMemory());
    }

    public static implicit operator StringRef(ReadOnlyMemory<char> value)
    {
        return value.IsEmpty ? Empty : new StringRef(value);
    }

    public static explicit operator string(StringRef value)
    {
        return value.ToString();
    }
}