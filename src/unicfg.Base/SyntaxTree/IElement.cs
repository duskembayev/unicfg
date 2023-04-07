namespace unicfg.Base.SyntaxTree;

public interface IElement
{
    void Accept(IElementVisitor visitor);
}