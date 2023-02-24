namespace ns2x.Parser.Builders;

internal sealed class AttributeBuilder : IValueOwner
{
    private readonly StringRef _name;
    private readonly ImmutableArray<IValue>.Builder _values;

    public AttributeBuilder(StringRef name)
    {
        _name = name;
        _values = ImmutableArray.CreateBuilder<IValue>(1);
    }

    public void AddValue(IValue value)
    {
        _values.Add(value);
    }

    public Attribute Build()
    {
        var value = BuildValue();
        return new Attribute(_name, value);
    }

    private IValue BuildValue()
    {
        return _values.Count switch
        {
            0 => EmptyValue.Instance,
            1 => _values[0],
            _ => new CollectionValue(_values.ToImmutable())
        };
    }
}