namespace unicfg.Model.Semantic;

public interface ISemanticNode
{
    void Accept(ISemanticNodeVisitor visitor);
}