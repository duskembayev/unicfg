using System.Diagnostics;
using unicfg.Model.Elements.Values;
using unicfg.Uni.Tree.Builders;
using unicfg.Uni.Tree.Extensions;

namespace unicfg.Uni.Tree.Handlers;

internal sealed class SymbolHandler : ISyntaxHandler
{
    private readonly SymbolBuilder _rootSymbolBuilder;

    public SymbolHandler(SymbolBuilder rootSymbolBuilder)
    {
        _rootSymbolBuilder = rootSymbolBuilder;
    }

    public bool CanHandle(in Token token)
    {
        return token.IsExpression();
    }

    public HandleResult Handle(ref TokenIndexer indexer)
    {
        try
        {
            var valueOwner = ResolveValueOwner(ref indexer);

            if (valueOwner is null)
                return HandleResult.ErrorResult(indexer.Token);

            if (!indexer.Token.IsEquality())
                return HandleResult.ErrorResult(indexer.Token);

            indexer.NextNotWhitespace();

            var value = ResolveValue(ref indexer);

            if (value is null)
            {
                valueOwner.SetValue(ErrorValue.Instance);
                return HandleResult.ErrorResult(indexer.Token);
            }

            valueOwner.SetValue(value);
        }
        finally
        {
            indexer.MoveTo(type => type >= TokenType.EndOfLine);
        }

        return HandleResult.SuccessResult;
    }

    private IValueOwner? ResolveValueOwner(ref TokenIndexer indexer)
    {
        var symbol = ResolveSymbol(ref indexer);

        if (symbol is null)
            return null;

        if (indexer.Token.IsBracketL())
            return ResolveAttribute(ref indexer, symbol);

        return symbol;
    }

    private SymbolBuilder? ResolveSymbol(ref TokenIndexer indexer)
    {
        var symbolStart = indexer.Token.RawRange.Start;
        var path = ResolvePath(ref indexer);

        if (path.IsEmpty)
            return null;

        var symbolEnd = indexer.Prev.Token.RawRange.End;
        var symbolBuilder = _rootSymbolBuilder;

        foreach (var name in path)
            symbolBuilder = symbolBuilder.AddSymbol(name, symbolStart..symbolEnd);

        return symbolBuilder;
    }

    private static AttributeBuilder? ResolveAttribute(ref TokenIndexer indexer, SymbolBuilder parentSymbol)
    {
        Debug.Assert(indexer.Token.IsBracketL());

        indexer.NextNotWhitespace();

        if (!indexer.TryReadContentTo(type => type != TokenType.Expression, out var attributeName))
        {
            return null;
        }

        indexer.SkipWhitespace();

        if (!indexer.Token.IsBracketR())
            return null;

        indexer.NextNotWhitespace();
        return parentSymbol.AddAttribute(attributeName);
    }

    private static IValue? ResolveValue(ref TokenIndexer indexer)
    {
        var valueBuilder = ImmutableArray.CreateBuilder<IValue>();

        while (!indexer.Token.IsEndOfLine())
        {
            var value = indexer.Token.Type switch
            {
                TokenType.Ref => CreateRefValue(ref indexer),
                TokenType.Unknown => CreateTextValue(in indexer),
                TokenType.Whitespace => CreateTextValue(in indexer),
                TokenType.Expression => CreateTextValue(in indexer),
                _ => null
            };

            if (value is null)
                return null;

            valueBuilder.Add(value);
            indexer = indexer.Next;
        }

        return valueBuilder.Count switch
        {
            0 => EmptyValue.Instance,
            1 => valueBuilder[0],
            _ => new CollectionValue(valueBuilder.ToImmutable())
        };
    }

    private static IValue CreateTextValue(in TokenIndexer indexer)
    {
        return new TextValue(indexer.Token.RawRange, indexer.Content);
    }

    private static IValue? CreateRefValue(ref TokenIndexer indexer)
    {
        var refTokenStart = indexer.Token.RawRange.Start;

        if (!TryParsePropertyRef(ref indexer, out var propertyRef))
            return null;

        return new RefValue(refTokenStart..indexer.Token.RawRange.End, propertyRef);
    }

    private static bool TryParsePropertyRef(ref TokenIndexer indexer, out SymbolRef propertyRef)
    {
        propertyRef = SymbolRef.Null;
        indexer = indexer.Next;

        if (!indexer.Token.IsBraceL())
            return false;

        indexer.NextNotWhitespace();

        var path = ResolvePath(ref indexer);

        if (path.IsEmpty || !indexer.Token.IsBraceR())
            return false;

        propertyRef = new SymbolRef(path);
        return true;
    }

    private static ImmutableArray<StringRef> ResolvePath(ref TokenIndexer indexer)
    {
        var pathBuilder = ImmutableArray.CreateBuilder<StringRef>();

        while (indexer.Token.IsExpression())
        {
            pathBuilder.Add(indexer.ReadContentTo(type => type != TokenType.Expression));
            indexer.SkipWhitespace();

            if (!indexer.Token.IsDot())
                break;

            indexer.NextNotWhitespace();
        }

        return pathBuilder.ToImmutable();
    }
}