using unicfg.Base.SemanticTree;

namespace unicfg.Formatters.Writers;

internal sealed class WriterAdapter : IEmitAsyncVisitor
{
    private readonly IWriter _innerWriter;

    public WriterAdapter(IWriter writer)
    {
        _innerWriter = writer;
    }

    async ValueTask IEmitAsyncVisitor.VisitScopeAsync(EmitScope scope, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (scope.Parent is not null && _innerWriter.CurrentScopeType is not ScopeType.Array)
        {
            _innerWriter.WritePropertyName(scope.Name.ToString());
        }

        if (scope.Attributes.TryGetValue(Attributes.Type, out var type)
            && type.Value.Equals("array"))
        {
            _innerWriter.WriteStartArray();
        }
        else
        {
            _innerWriter.WriteStartObject();
        }

        foreach (var (_, property) in scope.Properties)
        {
            await property.AcceptAsync(this, cancellationToken).ConfigureAwait(false);
        }

        foreach (var (_, child) in scope.Scopes)
        {
            await child.AcceptAsync(this, cancellationToken).ConfigureAwait(false);
        }

        switch (_innerWriter.CurrentScopeType)
        {
            case ScopeType.Array:
                _innerWriter.WriteEndArray();
                break;
            case ScopeType.Object:
                _innerWriter.WriteEndObject();
                break;
            default:
                throw new NotSupportedException($"Unsupported scope type: {_innerWriter.CurrentScopeType}");
        }
    }

    ValueTask IEmitAsyncVisitor.VisitPropertyAsync(EmitProperty property, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (_innerWriter.CurrentScopeType is not ScopeType.Array)
        {
            _innerWriter.WritePropertyName(property.Name.ToString());
        }

        WriteValue(property);

        return default;
    }

    private void WriteValue(EmitProperty property)
    {
        var value = property.Value?.Value.ToString();
        var valueType = property.Attributes
            .GetValueOrDefault(Attributes.Type)
            ?.Value.ToString();

        switch (valueType)
        {
            case "number" or "num" or "integer" or "int" or "long"
                when long.TryParse(value, out var longValue):
                _innerWriter.WriteValue(longValue);
                break;
            case "decimal" or "dec" or "float" or "double" or "real"
                when decimal.TryParse(value, out var decimalValue):
                _innerWriter.WriteValue(decimalValue);
                break;
            case "boolean" or "bool" or "bit"
                when bool.TryParse(value, out var boolValue):
                _innerWriter.WriteValue(boolValue);
                break;
            case "date" or "datetime" or "time" or "timestamp"
                when DateTime.TryParse(value, out var dateTimeValue):
                _innerWriter.WriteValue(dateTimeValue);
                break;
            case "raw"
                when value is not null:
                _innerWriter.WriteRaw(value);
                break;
            default:
                _innerWriter.WriteAuto(value);
                break;
        }
    }
}
