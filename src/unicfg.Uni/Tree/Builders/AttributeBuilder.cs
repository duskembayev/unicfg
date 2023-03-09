using unicfg.Model.Elements;
using unicfg.Model.Elements.Values;

namespace unicfg.Uni.Tree.Builders;

internal sealed class AttributeBuilder : IValueOwner
{
    private readonly StringRef _name;
    private readonly ImmutableArray<IValue>.Builder _values;

    public AttributeBuilder(StringRef name)
    {
        _name = name;
        _values = ImmutableArray.CreateBuilder<IValue>(1);
    }

    public void SetValue(IValue value)
    {
        _values.Add(value);
    }

    public UniAttribute Build(Document document, ElementWithName parent)
    {
        var result = new UniAttribute(_name, document, parent);
        result.Values = _values.ToImmutable();
        return result;
    }
}