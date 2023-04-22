using unicfg.Base.SyntaxTree.Values;

namespace unicfg.Uni.Tree.Builders;

internal interface IValueOwner
{
    void SetValue(IValue value);
}
