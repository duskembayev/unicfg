namespace unicfg.Base.SyntaxTree.Values;

public interface IValue : IElement
{
    Range SourceRange { get; }
}
