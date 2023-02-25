namespace ns2x.Model.Primitives;

public readonly record struct SourcePosition(int StartLine, int StartColumn, int EndLine, int EndColumn)
{
    public static readonly SourcePosition Null = default;
}