namespace unicfg.Base.Analysis;

public sealed record DiagnosticDescriptor(string Code, string Category, DiagnosticLevel Level, string MessageFormat)
{
    private const string LexerCategory = "Lexer";
    private const string ParserCategory = "Parser";
    private const string EvaluatorCategory = "Evaluator";
    private const string EmitterCategory = "Emitter";

    public static readonly DiagnosticDescriptor UnknownToken
        = new("UNI008", LexerCategory, DiagnosticLevel.Warn, "Unknown token");

    public static readonly DiagnosticDescriptor UnexpectedToken
        = new("UNI010", ParserCategory, DiagnosticLevel.Error, "Unexpected token");

    public static readonly DiagnosticDescriptor UnexpectedValueDeclaration
        = new("UNI011", ParserCategory, DiagnosticLevel.Warn, "Unexpected value declaration in \"{0}\"");

    public static readonly DiagnosticDescriptor UnexpectedSymbolDeclaration
        = new("UNI012", ParserCategory, DiagnosticLevel.Warn, "Unexpected symbol declaration in \"{0}\"");

    public static readonly DiagnosticDescriptor CircularReference
        = new("UNI017", EvaluatorCategory, DiagnosticLevel.Error, "Circular reference: \"{0}\"");

    public static readonly DiagnosticDescriptor UnresolvedReference
        = new("UNI018", EvaluatorCategory, DiagnosticLevel.Error, "Unresolved reference: \"{0}\"");

    public static readonly DiagnosticDescriptor ErrorReference
        = new("UNI019", EvaluatorCategory, DiagnosticLevel.Error, "Error reference: \"{0}\"");

    public static readonly DiagnosticDescriptor NothingToEmit
        = new("UNI501", EmitterCategory, DiagnosticLevel.Warn, "Nothing to emit");
}
