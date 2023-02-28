namespace ns2x.Model.Semantic;

public interface ISemanticNodeWithName : ISemanticNode
{
    StringRef Name { get; }
}