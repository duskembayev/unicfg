namespace unicfg.Base.Elements.Values;

public interface IValue : IElement
{
    Range SourceRange { get; }
}