using unicfg.Model.Semantic;

namespace unicfg.Parser.Builders;

internal interface IValueOwner
{
    void SetValue(IValue value);
}