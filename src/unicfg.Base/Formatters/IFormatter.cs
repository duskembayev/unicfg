using unicfg.Base.Primitives;
using unicfg.Base.SemanticTree;

namespace unicfg.Base.Formatters;

public interface IFormatter
{
    bool Matches(IReadOnlyDictionary<StringRef, StringRef> attributes);
    Task<EmitResult> FormatAsync(EmitScope scope, CancellationToken cancellationToken);
}