using unicfg.Base.SyntaxTree;
using unicfg.Base.SyntaxTree.Values;

namespace unicfg.Evaluation.Walkers;

internal abstract class VoidWalker : IElementVisitor<Void>
{
    public virtual Void Visit(Document document)
    {
        document.RootScope.Accept(this);
        return Void.Value;
    }

    public virtual Void Visit(ScopeSymbol scope)
    {
        foreach (var (_, element) in scope.Attributes)
        {
            element.Accept(this);
        }

        foreach (var (_, symbol) in scope.Children)
        {
            symbol.Accept(this);
        }

        return Void.Value;
    }

    public virtual Void Visit(PropertySymbol property)
    {
        foreach (var (_, element) in property.Attributes)
        {
            element.Accept(this);
        }

        property.Value.Accept(this);
        return Void.Value;
    }

    public virtual Void Visit(AttributeElement attribute)
    {
        attribute.Value.Accept(this);
        return Void.Value;
    }

    public virtual Void Visit(TextValue textValue)
    {
        return Void.Value;
    }

    public virtual Void Visit(RefValue refValue)
    {
        return Void.Value;
    }

    public virtual Void Visit(EmptyValue emptyValue)
    {
        return Void.Value;
    }

    public virtual Void Visit(ErrorValue errorValue)
    {
        return Void.Value;
    }

    public virtual Void Visit(CollectionValue collectionValue)
    {
        foreach (var value in collectionValue.Values)
        {
            value.Accept(this);
        }

        return Void.Value;
    }
}
