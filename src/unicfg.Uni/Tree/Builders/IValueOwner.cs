using unicfg.Base.Elements.Values;

namespace unicfg.Uni.Tree.Builders;

internal interface IValueOwner
{
    void SetValue(IValue value);
}