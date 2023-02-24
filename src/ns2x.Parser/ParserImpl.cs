using ns2x.Model.Diagnostics;
using ns2x.Parser.Builders;
using static ns2x.Model.Diagnostics.DiagnosticMessage;

namespace ns2x.Parser;

public sealed class ParserImpl
{
    private readonly List<(DiagnosticMessage, Token)> _diagnostics;
    private readonly SymbolBuilder _rootBuilder;

    public ParserImpl()
    {
        _diagnostics = new List<(DiagnosticMessage, Token)>();
        _rootBuilder = new SymbolBuilder(StringRef.Empty, SymbolKind.Namespace);
    }

    public Document Execute(ISource source, ImmutableArray<Token> tokens)
    {
        _diagnostics.Clear();

        var indexer = new TokenIndexer(0, source, tokens);

        while (!indexer.OutOfRange)
            if (!HandleRootToken(ref indexer))
                indexer = indexer.Next;

        return new Document(_rootBuilder.BuildAsNamespace());
    }

    /// <param name="indexer">any token</param>
    private bool HandleRootToken(ref TokenIndexer indexer)
    {
        if (indexer.Token.IsExpression())
            return HandlePropertyToken(ref indexer);

        if (!indexer.Token.IsHidden())
            ReportDiagnostics(indexer.Token, UnexpectedToken);

        return false;
    }

    /// <param name="indexer">expression token</param>
    private bool HandlePropertyToken(ref TokenIndexer indexer)
    {
        var symbolBuilder = _rootBuilder;
        var processed = false;

        while (indexer.Token.IsExpression())
        {
            symbolBuilder = symbolBuilder.AddSymbol(indexer.Text);
            indexer = indexer.Next;

            if (!indexer.Token.IsDot())
            {
                processed = true;
                break;
            }

            indexer = indexer.Next;
        }
        
        if (!processed || !indexer.Token.IsEquality())
        {
            ReportDiagnostics(indexer.Token, UnexpectedToken);
            return false;
        }

        return HandleValues(ref indexer, symbolBuilder);
    }

    /// <param name="indexer">equality token</param>
    /// <param name="owner"></param>
    private bool HandleValues(ref TokenIndexer indexer, IValueOwner owner)
    {
        indexer = indexer.Next;

        while (!indexer.Token.IsHidden())
        {
            var value = indexer.Token.IsRef() ? CreateRefValue(ref indexer) : CreateTextValue(in indexer);

            if (value is null)
                return false;

            owner.AddValue(value);
            indexer = indexer.Next;
        }

        return true;
    }

    /// <param name="indexer">any token</param>
    private static IValue CreateTextValue(in TokenIndexer indexer)
    {
        return new TextValue(indexer.Text);
    }

    /// <param name="indexer">ref token</param>
    private IValue? CreateRefValue(ref TokenIndexer indexer)
    {
        if (!TryParsePropertyRef(ref indexer, out var propertyRef))
            return null;

        return new RefValue(propertyRef);
    }

    /// <param name="indexer">ref token</param>
    /// <param name="propertyRef"></param>
    private bool TryParsePropertyRef(ref TokenIndexer indexer, out PropertyRef propertyRef)
    {
        propertyRef = PropertyRef.Null;
        indexer = indexer.Next;

        if (!indexer.Token.IsBraceL())
        {
            ReportDiagnostics(indexer.Token, UnexpectedToken);
            return false;
        }

        indexer = indexer.Next;
        
        var pathBuilder = ImmutableArray.CreateBuilder<StringRef>();
        var processed = false;

        while (indexer.Token.IsExpression())
        {
            pathBuilder.Add(indexer.Raw);
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
            ReportDiagnostics(indexer.Token, UnexpectedToken);
            return false;
        }

        propertyRef = new PropertyRef(pathBuilder.ToImmutable());
        return true;
    }

    private void ReportDiagnostics(in Token token, DiagnosticMessage message)
    {
        _diagnostics.Add((message, token));
    }
}