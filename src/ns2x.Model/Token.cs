namespace ns2x.Model;

public readonly record struct Token(Range Range, TokenType Type)
{
    public static readonly Token Eof = new(Range.All, TokenType.Eof);
    public static readonly Token Null = new(new Range(Index.End, Index.Start), TokenType.Unknown);

    public bool IsExpression()
    {
        return Type is TokenType.Expression or TokenType.QuotedExpression;
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

    public bool IsHidden()
    {
        return Type >= TokenType.Hidden;
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