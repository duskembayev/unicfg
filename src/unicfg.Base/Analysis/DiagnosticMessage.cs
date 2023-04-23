namespace unicfg.Base.Analysis;

public sealed record DiagnosticMessage(DiagnosticDescriptor Descriptor, object?[] Arguments)
{
    public string Text =>
        Arguments.Length > 0
            ? string.Format(Descriptor.Format, Arguments)
            : Descriptor.Format;

    public string? Location { get; init; }
    public DiagnosticPosition Position { get; init; } = DiagnosticPosition.Unknown;

    public override string ToString()
    {
        return $"[{Descriptor.Level:G} {Descriptor.Code}, {Descriptor.Category}] {Text}, {Position}";
    }
}
