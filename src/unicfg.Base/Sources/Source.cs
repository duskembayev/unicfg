using System.Text;

namespace unicfg.Base.Sources;

public static class Source
{
    public static ISource FromFile(string path)
    {
        return FromFile(path, Encoding.UTF8);
    }

    public static ISource FromFile(string path, Encoding encoding)
    {
        var location = Path.GetFullPath(path);
        var content = File.ReadAllText(path, encoding);

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