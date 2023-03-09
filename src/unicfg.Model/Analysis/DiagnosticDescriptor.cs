namespace unicfg.Model.Analysis;

public sealed record DiagnosticDescriptor(string Code, DiagnosticLevel Level, string MessageFormat)
{
    public static DiagnosticDescriptor UnknownToken = new("UNI008", DiagnosticLevel.Warn, "Unknown token");
    public static DiagnosticDescriptor UnexpectedToken = new("UNI010", DiagnosticLevel.Error, "Unexpected token");

    public static DiagnosticDescriptor UnexpectedValueDeclaration
        = new("UNI011", DiagnosticLevel.Warn, "Unexpected value declaration in \"{0}\"");
    public static DiagnosticDescriptor UnexpectedSymbolDeclaration
        = new("UNI012", DiagnosticLevel.Warn, "Unexpected symbol declaration in \"{0}\"");

    public static DiagnosticDescriptor CircularReference = new("UNI017", DiagnosticLevel.Error, "Circular reference: \"{0}\"");
    public static DiagnosticDescriptor UnresolvedReference = new("UNI018", DiagnosticLevel.Error, "Unresolved reference: \"{0}\"");
    public static DiagnosticDescriptor ErrorReference = new("UNI019", DiagnosticLevel.Error, "Error reference: \"{0}\"");
}