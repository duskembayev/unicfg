using unicfg.Base.Inputs;
using unicfg.Base.Primitives;

namespace unicfg.Uni.Tree;

internal readonly ref struct TokenIndexer
{
    private readonly ImmutableArray<Token> _tokens;

    public TokenIndexer(int index, ISource source, ImmutableArray<Token> tokens)
    {
        Index = index;
        Source = source;
        _tokens = tokens;
    }

    public ISource Source { get; }

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

    public StringRef Content => Source.GetText(Token.ContentRange);

}