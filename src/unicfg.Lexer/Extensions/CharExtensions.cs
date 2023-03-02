namespace unicfg.Lexer.Extensions;

public static class CharExtensions
{
    // The Unicode Standard, Sec. 5.8, Recommendation R4 and Table 5-2 state that the CR, LF,
    // CRLF, NEL, LS, FF, and PS sequences are considered newline functions. That section
    // also specifically excludes VT from the list of newline functions, so we do not include
    // it in the needle list.
    private const string EolChars = "\r\n\f\u0085\u2028\u2029";

    public static bool IsEol(this char @this)
    {
        for (var i = 0; i < EolChars.Length; i++)
            if (EolChars[i] == @this)
                return true;

        return false;
    }

    public static int IndexOfEol(this ReadOnlySpan<char> @this, out int stride)
    {
        stride = default;
        var idx = @this.IndexOfAny(EolChars);

        if ((uint) idx < (uint) @this.Length)
        {
            stride = 1; // needle found

            // Did we match CR? If so, and if it's followed by LF, then we need
            // to consume both chars as a single newline function match.

            if (@this[idx] == '\r')
            {
                var nextCharIdx = idx + 1;
                if ((uint) nextCharIdx < (uint) @this.Length && @this[nextCharIdx] == '\n') stride = 2;
            }
        }

        return idx;
    }
}