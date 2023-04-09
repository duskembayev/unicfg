using unicfg.Base.SyntaxTree;
using unicfg.Base.SyntaxTree.Values;

namespace unicfg.Evaluation.Walkers;

internal abstract class AbstractWalker : IElementVisitor
{
    public virtual void Visit(Document document)
    {
        document.RootScope.Accept(this);
    }

    public virtual void Visit(ScopeSymbol scope)
    {
        foreach (var (_, element) in scope.Attributes) element.Accept(this);
        foreach (var (_, symbol) in scope.Children) symbol.Accept(this);
    }

    public virtual void Visit(PropertySymbol property)
    {
        foreach (var (_, element) in property.Attributes) element.Accept(this);

        property.Value.Accept(this);
    }

    public virtual void Visit(AttributeElement attribute)
    {
        attribute.Value.Accept(this);
    }

    public virtual void Visit(TextValue textValue)
    {
    }

    public virtual void Visit(RefValue refValue)
    {
    }

    public virtual void Visit(EmptyValue emptyValue)
    {
    }

    public virtual void Visit(ErrorValue errorValue)
    {
    }

    public virtual void Visit(CollectionValue collectionValue)
    {
        foreach (var value in collectionValue.Values)
            value.Accept(this);
    }
}