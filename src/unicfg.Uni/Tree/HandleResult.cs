using System.Diagnostics.CodeAnalysis;

namespace unicfg.Uni.Tree;

internal readonly record struct HandleResult
{
    public static readonly HandleResult SuccessResult = new(true, null);

    private HandleResult(bool success, Token? errorToken)
    {
        this.Success = success;
        this.ErrorToken = errorToken;
    }

    [MemberNotNullWhen(false, nameof(ErrorToken))]
    public bool Success { get; }

    public Token? ErrorToken { get; }

    public static HandleResult ErrorResult(Token errorToken)
    {
        return new HandleResult(false, errorToken);
    }
}