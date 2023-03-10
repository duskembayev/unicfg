namespace unicfg.Base.Elements;

public interface IElement
{
    void Accept(IElementVisitor visitor);
}