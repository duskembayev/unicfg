namespace ns2x.Model.Semantic;

public interface ISemanticNodeWithValue : ISemanticNode
{
    public IValue Value => Values[^1];
    public ImmutableArray<IValue> Values { get; }
}