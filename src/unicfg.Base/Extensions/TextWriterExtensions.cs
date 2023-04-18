using unicfg.Base.Primitives;

namespace unicfg.Base.Extensions;

public static class TextWriterExtensions
{
    public static async ValueTask WriteAsync(
        this TextWriter @this,
        StringRef value,
        CancellationToken cancellationToken = default)
    {
        foreach (var memory in value.Memory.Segments)
            await @this.WriteAsync(memory, cancellationToken).ConfigureAwait(false);
    }

    public static void Write(this TextWriter @this, StringRef value)
    {
        foreach (var memory in value.Memory.Segments)
            @this.Write(memory);
    }
}