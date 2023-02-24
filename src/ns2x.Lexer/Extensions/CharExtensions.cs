namespace ns2x.Lexer.Extensions;

public static class CharExtensions
{
    public static bool IsEol(this char @this)
    {
        return @this is '\r' or '\n';
    }
}