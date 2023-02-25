using System.Collections;

namespace ns2x.Model.Analysis;

public class Diagnostics : IReadOnlyCollection<Diagnostic>
{
    private readonly ISource _source;
    private readonly List<Diagnostic> _diagnostics;

    public Diagnostics(ISource source)
    {
        _source = source;
        _diagnostics = new List<Diagnostic>();
    }

    public void Report(DiagnosticDescriptor descriptor)
    {
        Report(descriptor, in Token.Null, Array.Empty<object?>());
    }

    public void Report(DiagnosticDescriptor descriptor, in Token token)
    {
        Report(descriptor, in token, Array.Empty<object?>());
    }

    public void Report(DiagnosticDescriptor descriptor, in Token token, params object?[] args)
    {
        var (text, position) = GetTokenInfo(in token);

        _diagnostics.Add(new Diagnostic(descriptor, args)
        {
            Text = text,
            StartLine = position.StartLine,
            StartColumn = position.StartColumn
        });
    }

    private (StringRef text, SourcePosition) GetTokenInfo(in Token token)
    {
        if (token == Token.Eof || token == Token.Null)
            return (StringRef.Empty, SourcePosition.Null);

        return (_source.Raw(in token), _source.GetPosition(in token));
    }

    public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count => _diagnostics.Count;
}