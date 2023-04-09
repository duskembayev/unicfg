using unicfg.Base.Primitives;
using unicfg.Base.SemanticTree;

namespace unicfg.Base.Formatters;

public interface IFormatter
{
    bool Matches(IReadOnlyDictionary<StringRef, EmitValue> attributes);
    Task<EmitResult> FormatAsync(SymbolRef scopeRef, EmitScope scope, CancellationToken cancellationToken);
}