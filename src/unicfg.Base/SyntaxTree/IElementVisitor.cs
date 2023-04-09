using unicfg.Base.SyntaxTree.Values;

namespace unicfg.Base.SyntaxTree;

public interface IElementVisitor
{
    void Visit(Document document);
    void Visit(ScopeSymbol scope);
    void Visit(PropertySymbol property);
    void Visit(AttributeElement attribute);
    void Visit(TextValue textValue);
    void Visit(RefValue refValue);
    void Visit(EmptyValue emptyValue);
    void Visit(ErrorValue errorValue);
    void Visit(CollectionValue collectionValue);
}