namespace ns2x.Model.Analysis;

public sealed record DiagnosticDescriptor(string Code, DiagnosticLevel Level, string MessageFormat)
{
    public static DiagnosticDescriptor UnknownToken = new("NS008", DiagnosticLevel.Warn, "Unknown token");
    public static DiagnosticDescriptor UnexpectedToken = new("NS010", DiagnosticLevel.Error, "Unexpected token");
    public static DiagnosticDescriptor CircularReference = new("NS017", DiagnosticLevel.Error, "Circular reference: {0}");
    public static DiagnosticDescriptor UnresolvedReference = new("NS018", DiagnosticLevel.Error, "Unresolved reference: {0}");
    public static DiagnosticDescriptor ErrorReference = new("NS019", DiagnosticLevel.Error, "Error reference: {0}");
}