namespace unicfg.Model.Semantic;

public interface IValue : ISemanticNode
{
    Range SourceRange { get; }
}