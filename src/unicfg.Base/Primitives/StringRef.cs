using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace unicfg.Base.Primitives;

/// <summary>
/// Represents a reference to a string that is stored in a <see cref="MemoryRef{T}"/>
/// </summary>
public readonly struct StringRef
    : IEquatable<StringRef>,
      IEquatable<string>,
      IEquatable<ReadOnlyMemory<char>>,
      IComparable<StringRef>
{
    /// <summary>
    /// Empty <see cref="StringRef"/> instance.
    /// Can be used to avoid unnecessary allocations.
    /// </summary>
    public static readonly StringRef Empty = default;

    private readonly MemoryRef<char> _memory = MemoryRef<char>.Empty;

    private StringRef(MemoryRef<char> memory)
    {
        _memory = memory;
    }

    /// <summary>
    /// Returns memory segment that this <see cref="StringRef"/> is composed of.
    /// </summary>
    public MemoryRef<char> Memory => _memory;

    /// <summary>
    /// Compares with another <see cref="StringRef"/> for equality.
    /// </summary>
    /// <param name="other"></param>
    public bool Equals(StringRef other)
    {
        return _memory.Equals(other._memory);
    }

    /// <summary>
    /// Compares with another <see cref="ReadOnlyMemory{T}"/> for equality.
    /// </summary>
    /// <param name="other"></param>
    public bool Equals(ReadOnlyMemory<char> other)
    {
        return _memory.Equals(other);
    }

    /// <summary>
    /// Compares with another <see cref="string"/> for equality.
    /// </summary>
    /// <param name="other"></param>
    public bool Equals([NotNullWhen(true)] string? other)
    {
        return other is not null && _memory.Equals(other.AsMemory());
    }

    /// <summary>
    /// Compares with another <see cref="object"/> for equality.
    /// </summary>
    /// <param name="other"></param>
    public int CompareTo(StringRef other)
    {
        switch (IsEmpty, other.IsEmpty)
        {
            case (true, true):
                return 0;
            case (true, false):
                return -1;
            case (false, true):
                return 1;
        }

        var i = 0;

        while (i < Length && i < other.Length)
        {
            var c1 = this[i];
            var c2 = other[i];

            if (c1 != c2)
            {
                return c1 - c2;
            }

            i++;
        }

        return Length - other.Length;
    }

    /// <summary>
    /// Compares with another <see cref="object"/> for equality.
    /// </summary>
    /// <param name="obj"></param>
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

    /// <summary>
    /// Returns hash code of the <see cref="StringRef"/>.
    /// </summary>
    public override int GetHashCode()
    {
        return _memory.GetHashCode();
    }

    /// <summary>
    /// Returns string representation of the <see cref="StringRef"/>.
    /// </summary>
    public override string ToString()
    {
        if (_memory.IsEmpty)
        {
            return string.Empty;
        }

        var builder = new StringBuilder(Length);

        foreach (var memory in _memory.Segments)
        {
            builder.Append(memory);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Flag indicating whether the <see cref="StringRef"/> is empty.
    /// </summary>
    public bool IsEmpty => _memory.IsEmpty;

    /// <summary>
    /// Length of the <see cref="StringRef"/>.
    /// </summary>
    public int Length => _memory.Length;

    /// <summary>
    /// Returns character at the specified index.
    /// </summary>
    /// <param name="index"></param>
    public char this[Index index] => _memory[index];

    /// <summary>
    /// Returns substring of the <see cref="StringRef"/> at the specified range.
    /// </summary>
    /// <param name="range"></param>
    public StringRef this[Range range] => new(_memory[range]);

    /// <summary>
    /// Concatenates two <see cref="StringRef"/> instances.
    /// </summary>
    /// <param name="other"></param>
    public StringRef Concat(StringRef other)
    {
        return new StringRef(_memory.Concat(other._memory));
    }

    /// <summary>
    /// Implicitly converts <see cref="string"/> to <see cref="StringRef"/>.
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator StringRef(string? value)
    {
        return string.IsNullOrEmpty(value)
            ? Empty
            : new StringRef(MemoryRef<char>.From(value.AsMemory()));
    }

    /// <summary>
    /// Implicitly converts <see cref="ReadOnlyMemory{T}"/> to <see cref="StringRef"/>.
    /// </summary>
    /// <param name="value"></param>
    public static implicit operator StringRef(ReadOnlyMemory<char> value)
    {
        return value.IsEmpty
            ? Empty
            : new StringRef(MemoryRef<char>.From(value));
    }

    /// <summary>
    /// Explicitly converts <see cref="StringRef"/> to <see cref="string"/>.
    /// </summary>
    /// <param name="value"></param>
    public static explicit operator string(StringRef value)
    {
        return value.ToString();
    }

    /// <summary>
    /// Compares two <see cref="StringRef"/> instances.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public static bool operator ==(StringRef left, StringRef right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Compares two <see cref="StringRef"/> instances.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public static bool operator !=(StringRef left, StringRef right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Concatenates two <see cref="StringRef"/> instances.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public static StringRef operator +(StringRef left, StringRef right)
    {
        return left.Concat(right);
    }
}
