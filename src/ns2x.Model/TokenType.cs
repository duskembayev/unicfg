namespace ns2x.Model;

public enum TokenType : ushort
{
    Unknown,

    Dot,
    Ref,
    Equality,

    BraceL,
    BraceR,

    BracketL,
    BracketR,

    Expression,
    QuotedExpression,

    Hidden = 60000, // marker
    Comment = 60001,
    Eol = ushort.MaxValue - 1,
    Eof = ushort.MaxValue,
}