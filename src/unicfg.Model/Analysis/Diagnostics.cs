using System.Collections;
using unicfg.Model.Sources;

namespace unicfg.Model.Analysis;

public sealed class Diagnostics : IReadOnlyCollection<Diagnostic>
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
        Report(descriptor, Range.All, Array.Empty<object?>());
    }

    public void Report(DiagnosticDescriptor descriptor, in Range range)
    {
        Report(descriptor, in range, Array.Empty<object?>());
    }

    public void Report(DiagnosticDescriptor descriptor, in Range range, params object?[] args)
    {
        var (text, position) = GetRangeDetails(in range);

        _diagnostics.Add(new Diagnostic(descriptor, args)
        {
            Text = text,
            StartLine = position.StartLine,
            StartColumn = position.StartColumn
        });
    }

    private (StringRef text, SourcePosition) GetRangeDetails(in Range range)
    {
        if (range.Equals(Range.All))
            return (StringRef.Empty, SourcePosition.Null);

        return (_source.GetText(in range), _source.GetPosition(in range));
    }

    public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count => _diagnostics.Count;
}