using unicfg.Model.Elements;
using unicfg.Model.Sources;
using unicfg.Uni.Tree.Builders;
using unicfg.Uni.Tree.Handlers;
using static unicfg.Model.Analysis.DiagnosticDescriptor;

namespace unicfg.Uni.Tree;

public sealed class ParserImpl
{
    private readonly Diagnostics _diagnostics;
    private readonly SymbolBuilder _rootBuilder;
    private readonly ImmutableArray<ISyntaxHandler> _handlers;

    public ParserImpl(Diagnostics diagnostics)
    {
        _diagnostics = diagnostics;
        _rootBuilder = new SymbolBuilder(StringRef.Empty, SymbolKind.PropertyGroup, _diagnostics);

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
        document.RootGroup = _rootBuilder.BuildAsNamespace(document, null);
        return document;
    }

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