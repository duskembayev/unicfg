using unicfg.Base.Primitives;

namespace unicfg.Base.Analysis;

public readonly record struct DiagnosticPosition(int Line, int Column, StringRef Text)
{
    public static readonly DiagnosticPosition Unknown = new(-1, -1, StringRef.Empty);

    public override string ToString()
    {
        return $"({Line}:{Column} = \"{Text}\")";
    }
}
