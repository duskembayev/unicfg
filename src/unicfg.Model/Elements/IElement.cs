namespace unicfg.Model.Elements;

public interface IElement
{
    void Accept(IElementVisitor visitor);
}