namespace unicfg.Base.Analysis;

public sealed record Diagnostic(DiagnosticDescriptor Descriptor, object?[] Arguments)
{
    public string Message =>
        Arguments.Length > 0
            ? string.Format(Descriptor.MessageFormat, Arguments)
            : Descriptor.MessageFormat;

    public string? Location { get; init; }
    public DiagnosticPosition Position { get; init; } = DiagnosticPosition.Unknown;

    public override string ToString()
    {
        return $"[{Descriptor.Level:G} {Descriptor.Code}, {Descriptor.Category}] {Message}, {Position}";
    }
}
