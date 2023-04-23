using unicfg.Base.Inputs;

namespace unicfg.Base.Analysis;

public interface IDiagnostics : IReadOnlyCollection<DiagnosticMessage>
{
    void Report(DiagnosticDescriptor descriptor);
    void Report(DiagnosticDescriptor descriptor, object?[] args);
    void Report(DiagnosticDescriptor descriptor, string location);
    void Report(DiagnosticDescriptor descriptor, string location, object?[] args);
    void Report(DiagnosticDescriptor descriptor, ISource source, Range range);
    void Report(DiagnosticDescriptor descriptor, ISource source, Range range, object?[] args);
    void Report(DiagnosticDescriptor descriptor, Range range);
    void Report(DiagnosticDescriptor descriptor, Range range, object?[] args);
    
    IDiagnostics WithSource(ISource source);
}
