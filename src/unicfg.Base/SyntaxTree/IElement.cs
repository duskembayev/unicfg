namespace unicfg.Base.SyntaxTree;

public interface IElement
{
    T Accept<T>(IElementVisitor<T> visitor);
}
