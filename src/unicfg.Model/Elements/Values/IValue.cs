namespace unicfg.Model.Elements.Values;

public interface IValue : IElement
{
    Range SourceRange { get; }
}