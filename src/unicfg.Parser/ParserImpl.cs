using unicfg.Model;
using unicfg.Model.Analysis;
using unicfg.Model.Primitives;
using unicfg.Model.Semantic;
using unicfg.Parser.Builders;
using unicfg.Parser.Handlers;
using static unicfg.Model.Analysis.DiagnosticDescriptor;

namespace unicfg.Parser;

public sealed class ParserImpl
{
    private readonly Diagnostics _diagnostics;
    private readonly SymbolBuilder _rootBuilder;
    private readonly ImmutableArray<ISyntaxHandler> _handlers;

    public ParserImpl(Diagnostics diagnostics)
    {
        _diagnostics = diagnostics;
        _rootBuilder = new SymbolBuilder(StringRef.Empty, SymbolKind.Namespace, _diagnostics);

        _handlers = ImmutableArray.Create<ISyntaxHandler>(
            new SymbolHandler(_rootBuilder),
            new EolHandler(),
            new WhitespaceHandler());
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
        foreach (var handler in _handlers)
            if (handler.CanHandle(in indexer.Token))
            {
                var result = handler.Handle(ref indexer);

                if (!result.Success)
                    _diagnostics.Report(UnexpectedToken, result.ErrorToken.Value.RawRange);

                return true;
            }

        _diagnostics.Report(UnexpectedToken, indexer.Token.RawRange);
        return false;
    }
}