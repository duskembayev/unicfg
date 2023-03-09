using unicfg.Model.Elements.Values;

namespace unicfg.Model.Elements;

public interface IElementVisitor
{
    void Visit(Document document);
    void Visit(UniPropertyGroup group);
    void Visit(UniProperty property);
    void Visit(UniAttribute attribute);
    void Visit(TextValue textValue);
    void Visit(RefValue refValue);
    void Visit(EmptyValue emptyValue);
    void Visit(ErrorValue errorValue);
    void Visit(CollectionValue collectionValue);
}