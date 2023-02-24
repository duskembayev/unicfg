namespace ns2x.Model.Semantic;

public interface ISemanticNode
{
    void Accept(ISemanticNodeVisitor visitor);
}