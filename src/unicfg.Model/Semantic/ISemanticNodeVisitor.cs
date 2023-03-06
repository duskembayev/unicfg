namespace unicfg.Model.Semantic;

public interface ISemanticNodeVisitor
{
    void Visit(Document document);
    void Visit(Namespace ns);
    void Visit(Property property);
    void Visit(Attribute attribute);
    void Visit(TextValue textValue);
    void Visit(RefValue refValue);
    void Visit(EmptyValue emptyValue);
    void Visit(ErrorValue errorValue);
    void Visit(CollectionValue collectionValue);
}