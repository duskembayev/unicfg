using unicfg.Model.Sources;

namespace unicfg.Uni.Tree;

internal readonly ref struct TokenIndexer
{
    private readonly ISource _source;
    private readonly ImmutableArray<Token> _tokens;

    public TokenIndexer(int index, ISource source, ImmutableArray<Token> tokens)
    {
        Index = index;
        _source = source;
        _tokens = tokens;
    }

    private int Index { get; init; }

    public bool OutOfRange => Index < 0 || Index >= _tokens.Length;

    public ref readonly Token Token
    {
        get
        {
            if (OutOfRange)
                throw new InvalidOperationException();

            return ref _tokens.ItemRef(Index);
        }
    }

    public TokenIndexer Next => this with { Index = Index + 1 };
    public TokenIndexer Prev => this with { Index = Index - 1 };

    public StringRef Content => _source.GetText(Token.ContentRange);

}