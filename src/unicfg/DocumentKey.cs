namespace unicfg;

public readonly struct DocumentKey : IEquatable<DocumentKey>
{
    public static readonly DocumentKey Environment = new("25FFF122-26C5-4878-8286-F2687DA8C34C");
    public static readonly DocumentKey Args = new("F94B645D-D296-4A3A-AE98-160DBB53BF60");

    private readonly string _key;

    private DocumentKey(string key)
    {
        _key = key;
    }

    public static DocumentKey FromLocation(string filePath)
    {
        return new DocumentKey(filePath);
    }

    public bool Equals(DocumentKey other)
    {
        return string.Equals(_key, other._key, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return obj is DocumentKey other && Equals(other);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(_key);
    }
}