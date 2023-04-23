namespace unicfg.Evaluation;

internal static class DiagnosticDescriptors
{
    private const string EvaluatorCategory = "Evaluator";
    private const string EmitterCategory = "Emitter";

    internal static readonly DiagnosticDescriptor CircularReference
        = new("UNI017", EvaluatorCategory, DiagnosticLevel.Error, "Circular reference: \"{0}\"");

    internal static readonly DiagnosticDescriptor UnresolvedReference
        = new("UNI018", EvaluatorCategory, DiagnosticLevel.Error, "Unresolved reference: \"{0}\"");

    internal static readonly DiagnosticDescriptor ErrorReference
        = new("UNI019", EvaluatorCategory, DiagnosticLevel.Error, "Error reference: \"{0}\"");

    internal static readonly DiagnosticDescriptor NothingToEmit
        = new("UNI501", EmitterCategory, DiagnosticLevel.Warn, "Nothing to emit");
}
