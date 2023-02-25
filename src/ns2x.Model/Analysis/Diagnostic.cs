using System.Text.RegularExpressions;

namespace ns2x.Model.Analysis;

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
        var text = Regex.Escape(Text.ToString());
        return $"[{Descriptor.Level:G}] {Message}, ({StartLine}:{StartColumn}) = \"{text}\"";
    }
}