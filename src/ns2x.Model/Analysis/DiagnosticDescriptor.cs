namespace ns2x.Model.Analysis;

public sealed record DiagnosticDescriptor(string Code, DiagnosticLevel Level, string MessageFormat)
{
    public static DiagnosticDescriptor UnknownToken = new("NS008", DiagnosticLevel.Warn, "Unknown token");
    public static DiagnosticDescriptor UnexpectedToken = new("NS010", DiagnosticLevel.Error, "Unexpected token");
}