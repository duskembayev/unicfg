using System.Text;

namespace unicfg.Base.Inputs;

public static class Source
{
    public static Task<ISource> FromFileAsync(string path, CancellationToken cancellationToken = default)
    {
        return FromFileAsync(path, Encoding.UTF8);
    }

    public static async Task<ISource> FromFileAsync(
        string path,
        Encoding encoding,
        CancellationToken cancellationToken = default)
    {
        var location = Path.GetFullPath(path);
        var content = await File.ReadAllTextAsync(path, encoding, cancellationToken).ConfigureAwait(false);

        return new MemorySource(content.AsMemory())
        {
            Location = location
        };
    }

    public static ISource Create(string input)
    {
        return new MemorySource(input.AsMemory());
    }
}
