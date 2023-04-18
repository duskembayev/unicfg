using System.Collections.ObjectModel;
using unicfg.Base.Primitives;

namespace unicfg.Base.SemanticTree;

public abstract class EmitSymbol
{
    private static readonly IReadOnlyDictionary<StringRef, EmitValue> EmptyAttributes
        = new ReadOnlyDictionary<StringRef, EmitValue>(new Dictionary<StringRef, EmitValue>());

    private IReadOnlyDictionary<StringRef, EmitValue>? _readOnlyAttributes;
    private IDictionary<StringRef, EmitValue>? _writableAttributes;

    protected EmitSymbol(StringRef name)
    {
        Name = name;

        _readOnlyAttributes = null;
        _writableAttributes = null;
    }

    public StringRef Name { get; }
    public EmitScope? Parent { get; init; }

    public IReadOnlyDictionary<StringRef, EmitValue> Attributes
    {
        get
        {
            if (_writableAttributes is null)
                return EmptyAttributes;

            return _readOnlyAttributes ??= new ReadOnlyDictionary<StringRef, EmitValue>(_writableAttributes);
        }
    }

    public void SetAttributeValue(StringRef name, EmitValue value)
    {
        if (name.IsEmpty)
            throw new InvalidOperationException();

        _writableAttributes ??= new Dictionary<StringRef, EmitValue>();
        _writableAttributes[name] = value;
    }

    public abstract ValueTask AcceptAsync(IEmitAsyncVisitor visitor, CancellationToken cancellationToken);
}