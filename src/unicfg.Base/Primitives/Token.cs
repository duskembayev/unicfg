namespace unicfg.Base.Primitives;

public readonly record struct Token(TokenType Type, Range RawRange, Range ContentRange)
{
    public static readonly Token Eof = new(TokenType.Eof, Range.All);
    public static readonly Token Null = new(TokenType.Unknown, new Range(Index.End, Index.Start));

    public Token(TokenType type, Range rawRange)
        : this(type, rawRange, rawRange)
    {
    }

    public bool IsExpression()
    {
        return Type is TokenType.Expression;
    }

    public bool IsDot()
    {
        return Type is TokenType.Dot;
    }

    public bool IsRef()
    {
        return Type is TokenType.Ref;
    }

    public bool IsEquality()
    {
        return Type is TokenType.Equality;
    }

    public bool IsWhitespace()
    {
        return Type is TokenType.Whitespace;
    }

    public bool IsEndOfLine()
    {
        return Type >= TokenType.EndOfLine;
    }

    public bool IsBraceL()
    {
        return Type is TokenType.BraceL;
    }

    public bool IsBraceR()
    {
        return Type is TokenType.BraceR;
    }

    public bool IsBracketL()
    {
        return Type is TokenType.BracketL;
    }

    public bool IsBracketR()
    {
        return Type is TokenType.BracketR;
    }
}
