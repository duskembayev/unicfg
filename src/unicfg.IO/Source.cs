using System.Text;
using unicfg.Model;

namespace unicfg.IO;

public static class Source
{
    public static ISource FromFile(string path)
    {
        return FromFile(path, Encoding.UTF8);
    }

    public static ISource FromFile(string path, Encoding encoding)
    {
        return Create(File.ReadAllText(path, encoding));
    }

    public static ISource Create(string input)
    {
        return new MemorySource(input.AsMemory());
    }
}