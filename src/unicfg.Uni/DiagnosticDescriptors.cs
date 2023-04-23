namespace unicfg.Uni;

internal static class DiagnosticDescriptors
{
    private const string LexerCategory = "Lexer";
    private const string ParserCategory = "Parser";

    internal static readonly DiagnosticDescriptor UnknownToken
        = new("UNI008", LexerCategory, DiagnosticLevel.Warn, "Unknown token");

    internal static readonly DiagnosticDescriptor UnexpectedToken
        = new("UNI010", ParserCategory, DiagnosticLevel.Error, "Unexpected token");

    internal static readonly DiagnosticDescriptor UnexpectedValueDeclaration
        = new("UNI011", ParserCategory, DiagnosticLevel.Warn, "Unexpected value declaration in \"{0}\"");

    internal static readonly DiagnosticDescriptor UnexpectedSymbolDeclaration
        = new("UNI012", ParserCategory, DiagnosticLevel.Warn, "Unexpected symbol declaration in \"{0}\"");
}
