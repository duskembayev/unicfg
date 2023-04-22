using System.Collections;
using System.Collections.Concurrent;
using Enhanced.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using unicfg.Base.Inputs;

namespace unicfg.Base.Analysis;

[ContainerEntry(ServiceLifetime.Scoped, typeof(Diagnostics))]
public sealed class Diagnostics : IReadOnlyCollection<Diagnostic>
{
    private readonly ConcurrentBag<Diagnostic> _bag;
    private readonly ISource? _source;

    public Diagnostics() : this(null, new ConcurrentBag<Diagnostic>())
    {
    }

    private Diagnostics(ISource? source, ConcurrentBag<Diagnostic> bag)
    {
        _source = source;
        _bag = bag;
    }

    public IEnumerator<Diagnostic> GetEnumerator()
    {
        return _bag.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count => _bag.Count;

    public void Report(DiagnosticDescriptor descriptor)
    {
        Report(descriptor, Array.Empty<object>());
    }

    public void Report(DiagnosticDescriptor descriptor, object?[] args)
    {
        ReportCore(new Diagnostic(descriptor, args));
    }

    public void Report(DiagnosticDescriptor descriptor, string location)
    {
        Report(descriptor, location, Array.Empty<object>());
    }

    public void Report(DiagnosticDescriptor descriptor, string location, object?[] args)
    {
        ReportCore(new Diagnostic(descriptor, args) { Location = location });
    }

    public void Report(DiagnosticDescriptor descriptor, ISource source, Range range)
    {
        Report(descriptor, source, range, Array.Empty<object>());
    }

    public void Report(DiagnosticDescriptor descriptor, ISource source, Range range, object?[] args)
    {
        ReportCore(
            new Diagnostic(descriptor, args)
            {
                Location = source.Location,
                Position = GetDiagnosticPosition(source, range)
            });
    }

    public void Report(DiagnosticDescriptor descriptor, Range range)
    {
        Report(descriptor, range, Array.Empty<object>());
    }

    public void Report(DiagnosticDescriptor descriptor, Range range, object?[] args)
    {
        if (_source is null)
        {
            throw new NotSupportedException();
        }

        ReportCore(
            new Diagnostic(descriptor, args)
            {
                Location = _source.Location,
                Position = GetDiagnosticPosition(_source, range)
            });
    }

    public Diagnostics WithSource(ISource source)
    {
        return new Diagnostics(source, _bag);
    }

    private void ReportCore(Diagnostic diagnostic)
    {
        _bag.Add(diagnostic);
    }

    private static DiagnosticPosition GetDiagnosticPosition(ISource source, Range range)
    {
        if (range.Equals(Range.All))
        {
            return DiagnosticPosition.Unknown;
        }

        var text = source.GetText(range);
        var (startLine, startColumn, _, _) = source.GetPosition(in range);

        return new DiagnosticPosition(startLine, startColumn, text);
    }
}
