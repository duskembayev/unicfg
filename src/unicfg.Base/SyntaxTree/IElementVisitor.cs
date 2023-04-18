using unicfg.Base.SyntaxTree.Values;

namespace unicfg.Base.SyntaxTree;

public interface IElementVisitor<out T>
{
    T Visit(Document document);
    T Visit(ScopeSymbol scope);
    T Visit(PropertySymbol property);
    T Visit(AttributeElement attribute);
    T Visit(TextValue textValue);
    T Visit(RefValue refValue);
    T Visit(EmptyValue emptyValue);
    T Visit(ErrorValue errorValue);
    T Visit(CollectionValue collectionValue);
}