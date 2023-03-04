namespace unicfg.Model;

public enum TokenType : ushort
{
    Unknown,
    Reserved = Unknown,
    Whitespace,

    Dot,
    Ref,
    Keyword,
    Equality,

    BraceL,
    BraceR,

    BracketL,
    BracketR,

    Expression,

    EndOfLine = 500, // marker
    Comment,
    Eol,
    Eof,
}