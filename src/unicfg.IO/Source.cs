using System.Text;
using unicfg.Model;

namespace unicfg.IO;

public static class Source
{
    public static ISource FromFile(string path, Encoding encoding)
    {
        return new MemorySource(File.ReadAllText(path, encoding).AsMemory());
    }

    public static ISource FromFile(string path)
    {
        return FromFile(path, Encoding.UTF8);
    }
}