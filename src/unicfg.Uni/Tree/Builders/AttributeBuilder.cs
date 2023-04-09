using unicfg.Base.Primitives;
using unicfg.Base.SyntaxTree;
using unicfg.Base.SyntaxTree.Values;

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

    public AttributeElement Build(ISymbol parent)
    {
        return new AttributeElement(_name, parent, _values.ToImmutable());
    }
}