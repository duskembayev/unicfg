using System.Diagnostics.CodeAnalysis;
using unicfg.Base.Primitives;

namespace unicfg.Uni.Tree;

internal readonly record struct HandleResult
{
    public static readonly HandleResult SuccessResult = new(true, null);

    private HandleResult(bool success, Token? errorToken)
    {
        Success = success;
        ErrorToken = errorToken;
    }

    [MemberNotNullWhen(false, nameof(ErrorToken))]
    public bool Success { get; }

    public Token? ErrorToken { get; }

    public static HandleResult ErrorResult(Token errorToken)
    {
        return new HandleResult(false, errorToken);
    }
}
