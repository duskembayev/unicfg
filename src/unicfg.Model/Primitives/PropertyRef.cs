using System.Text;

namespace unicfg.Model.Primitives;

public readonly struct PropertyRef : IEquatable<PropertyRef>
{
    public static readonly PropertyRef Null = default;

    public PropertyRef(ImmutableArray<StringRef> path)
    {
        Path = path;
    }

    public ImmutableArray<StringRef> Path { get; }

    public bool Equals(PropertyRef other)
    {
        if (other.Path.Length != Path.Length)
            return false;

        var index = -1;
        var result = true;

        while (result && ++index < other.Path.Length)
            result &= other.Path[index].Equals(Path[index]);

        return result;
    }

    public override bool Equals(object? obj)
    {
        return obj is PropertyRef other && Equals(other);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        for (var index = 0; index < Path.Length; index++)
            hashCode.Add(Path[index]);

        return hashCode.ToHashCode();
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        for (var index = 0; index < Path.Length; index++)
        {
            if (index > 0)
                builder.Append('.');

            builder.Append((string) Path[index]);
        }

        return builder.ToString();
    }
}