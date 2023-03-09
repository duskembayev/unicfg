namespace unicfg.Model.Analysis;

public sealed record Diagnostic(DiagnosticDescriptor Descriptor, object?[] Arguments)
{
    public string Message => Arguments.Length > 0
        ? string.Format(Descriptor.MessageFormat, Arguments)
        : Descriptor.MessageFormat;

    public StringRef Text { get; init; } = StringRef.Empty;

    public int StartLine { get; init; } = -1;
    public int StartColumn { get; init; } = -1;

    public override string ToString()
    {
        return $"[{Descriptor.Level:G} {Descriptor.Code}] {Message}, ({StartLine}:{StartColumn}) = \"{Text}\"";
    }
}