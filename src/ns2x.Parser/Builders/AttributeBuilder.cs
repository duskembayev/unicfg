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

    public void SetValue(IValue value)
    {
        _values.Add(value);
    }

    public Attribute Build()
    {
        return new Attribute(_name, _values.ToImmutable());
    }
}