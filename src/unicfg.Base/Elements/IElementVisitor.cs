using unicfg.Base.Elements.Values;

namespace unicfg.Base.Elements;

public interface IElementVisitor
{
    void Visit(Document document);
    void Visit(UniScope scope);
    void Visit(UniProperty property);
    void Visit(UniAttribute attribute);
    void Visit(TextValue textValue);
    void Visit(RefValue refValue);
    void Visit(EmptyValue emptyValue);
    void Visit(ErrorValue errorValue);
    void Visit(CollectionValue collectionValue);
}