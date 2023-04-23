using Microsoft.Extensions.Logging;
using unicfg.Base.SyntaxTree.Values;

namespace unicfg.Base.SyntaxTree.Walkers;

public abstract class AsyncWalker : IElementVisitor<ValueTask>
{
    private readonly CancellationToken _cancellationToken;

    protected AsyncWalker(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
    }

    public virtual async ValueTask Visit(Document document)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        await document.RootScope.Accept(this).ConfigureAwait(false);
    }

    public virtual async ValueTask Visit(ScopeSymbol scope)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        foreach (var (_, element) in scope.Attributes)
        {
            await element.Accept(this).ConfigureAwait(false);
        }

        foreach (var (_, symbol) in scope.Children)
        {
            await symbol.Accept(this).ConfigureAwait(false);
        }
    }

    public virtual async ValueTask Visit(PropertySymbol property)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        foreach (var (_, element) in property.Attributes)
        {
            await element.Accept(this).ConfigureAwait(false);
        }

        await property.Value.Accept(this).ConfigureAwait(false);
    }

    public virtual async ValueTask Visit(AttributeElement attribute)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        await attribute.Value.Accept(this).ConfigureAwait(false);
    }

    public virtual ValueTask Visit(TextValue textValue)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        return ValueTask.CompletedTask;
    }

    public virtual ValueTask Visit(RefValue refValue)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        return ValueTask.CompletedTask;
    }

    public virtual ValueTask Visit(EmptyValue emptyValue)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        return ValueTask.CompletedTask;
    }

    public virtual ValueTask Visit(ErrorValue errorValue)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        return ValueTask.CompletedTask;
    }

    public virtual async ValueTask Visit(CollectionValue collectionValue)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        foreach (var value in collectionValue.Values)
        {
            await value.Accept(this).ConfigureAwait(false);
        }
    }
}
