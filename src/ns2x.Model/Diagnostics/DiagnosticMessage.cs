namespace ns2x.Model.Diagnostics;

public sealed record DiagnosticMessage(string Code, string Message)
{
    public static DiagnosticMessage UnexpectedToken = new("NS010", "Unexpected token");
}
