using unicfg.Model;
using unicfg.Model.Analysis;
using unicfg.Model.Primitives;
using unicfg.Model.Semantic;
using unicfg.Parser.Builders;
using static unicfg.Model.Analysis.DiagnosticDescriptor;

namespace unicfg.Parser;

public sealed class ParserImpl
{
    private readonly Diagnostics _diagnostics;
    private readonly SymbolBuilder _rootBuilder;

    public ParserImpl(Diagnostics diagnostics)
    {
        _diagnostics = diagnostics;
        _rootBuilder = new SymbolBuilder(StringRef.Empty, SymbolKind.Namespace);
    }

    public Document Execute(ISource source, ImmutableArray<Token> tokens)
    {
        var indexer = new TokenIndexer(0, source, tokens);

        while (!indexer.OutOfRange)
            if (!HandleRootToken(ref indexer))
                indexer = indexer.Next;

        var document = new Document();
        document.RootNamespace = _rootBuilder.BuildAsNamespace(document, null);
        return document;
    }

    /// <param name="indexer">any token</param>
    private bool HandleRootToken(ref TokenIndexer indexer)
    {
        if (indexer.Token.IsExpression())
            return HandlePropertyToken(ref indexer);

        if (!indexer.Token.IsHidden())
            Report(UnexpectedToken, indexer.Token);

        return false;
    }

    /// <param name="indexer">expression token</param>
    private bool HandlePropertyToken(ref TokenIndexer indexer)
    {
        var symbolBuilder = _rootBuilder;
        var processed = false;

        while (indexer.Token.IsExpression())
        {
            symbolBuilder = symbolBuilder.AddSymbol(indexer.RawText);
            indexer = indexer.Next;

            if (!indexer.Token.IsDot())
            {
                processed = true;
                break;
            }

            indexer = indexer.Next;
        }

        if (!processed)
        {
            Report(UnexpectedToken, indexer.Token);
            return false;
        }

        IValueOwner? valueOwner = symbolBuilder;

        if (indexer.Token.IsBracketL())
        {
            valueOwner = HandleAttributeToken(ref indexer, symbolBuilder);

            if (valueOwner is null)
                return false;

            indexer = indexer.Next;
        }

        if (!indexer.Token.IsEquality())
        {
            Report(UnexpectedToken, indexer.Token);
            return false;
        }

        return HandleValues(ref indexer, valueOwner);
    }

    /// <param name="indexer">bracketL token</param>
    /// <param name="symbolBuilder"></param>
    private AttributeBuilder? HandleAttributeToken(ref TokenIndexer indexer, SymbolBuilder symbolBuilder)
    {
        indexer = indexer.Next;

        if (!indexer.Token.IsExpression())
        {
            Report(UnexpectedToken, indexer.Token);
            return null;
        }

        var attributeName = indexer.RawText;
        indexer = indexer.Next;

        if (!indexer.Token.IsBracketR())
        {
            Report(UnexpectedToken, indexer.Token);
            return null;
        }


        return symbolBuilder.AddAttribute(attributeName);
    }

    /// <param name="indexer">equality token</param>
    /// <param name="owner"></param>
    private bool HandleValues(ref TokenIndexer indexer, IValueOwner owner)
    {
        indexer = indexer.Next;

        var valueBuilder = ImmutableArray.CreateBuilder<IValue>();

        while (!indexer.Token.IsHidden())
        {
            var value = indexer.Token.IsRef() ? CreateRefValue(ref indexer) : CreateTextValue(in indexer);

            if (value is null)
                return false;

            valueBuilder.Add(value);
            indexer = indexer.Next;
        }

        owner.SetValue(CreateValue(valueBuilder.ToImmutable()));
        return true;
    }
    
    private static IValue CreateValue(ImmutableArray<IValue> valueParts)
    {
        return valueParts.Length switch
        {
            0 => EmptyValue.Instance,
            1 => valueParts[0],
            _ => new CollectionValue(valueParts)
        };
    }

    /// <param name="indexer">any token</param>
    private static IValue CreateTextValue(in TokenIndexer indexer)
    {
        return new TextValue(indexer.Token.Range, indexer.Text);
    }

    /// <param name="indexer">ref token</param>
    private IValue? CreateRefValue(ref TokenIndexer indexer)
    {
        var refTokenStart = indexer.Token.Range.Start;

        if (!TryParsePropertyRef(ref indexer, out var propertyRef))
            return null;

        return new RefValue(refTokenStart..indexer.Token.Range.End, propertyRef);
    }

    /// <param name="indexer">ref token</param>
    /// <param name="propertyRef"></param>
    private bool TryParsePropertyRef(ref TokenIndexer indexer, out PropertyRef propertyRef)
    {
        propertyRef = PropertyRef.Null;
        indexer = indexer.Next;

        if (!indexer.Token.IsBraceL())
        {
            Report(UnexpectedToken, indexer.Token);
            return false;
        }

        indexer = indexer.Next;

        var pathBuilder = ImmutableArray.CreateBuilder<StringRef>();
        var processed = false;

        while (indexer.Token.IsExpression())
        {
            pathBuilder.Add(indexer.RawText);
            indexer = indexer.Next;

            if (!indexer.Token.IsDot())
            {
                processed = true;
                break;
            }

            indexer = indexer.Next;
        }

        if (!processed || !indexer.Token.IsBraceR())
        {
            Report(UnexpectedToken, indexer.Token);
            return false;
        }

        propertyRef = new PropertyRef(pathBuilder.ToImmutable());
        return true;
    }

    private void Report(DiagnosticDescriptor descriptor, in Token token)
    {
        _diagnostics.Report(descriptor, token.Range);
    }
}