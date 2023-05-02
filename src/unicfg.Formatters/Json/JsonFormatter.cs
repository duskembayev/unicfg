using unicfg.Base.Formatters;
using unicfg.Base.Primitives;
using unicfg.Base.SemanticTree;
using unicfg.Formatters.Writers;

namespace unicfg.Formatters.Json;

public sealed class JsonFormatter : IFormatter
{
    private static readonly string[] JsonKeywords = { "json" };
    private static readonly string[] JsonExtensions = { ".json" };
    private readonly DirectoryInfo _outputDirectory;

    public JsonFormatter(DirectoryInfo outputDirectory)
    {
        _outputDirectory = outputDirectory;
    }

    public bool Matches(IReadOnlyDictionary<StringRef, EmitValue> attributes)
    {
        if (attributes.TryGetValue(Attributes.Format, out var format))
        {
            return JsonKeywords.Contains(format.Value.ToString(), StringComparer.OrdinalIgnoreCase);
        }

        if (attributes.TryGetValue(Attributes.Output, out var output))
        {
            var extension = Path.GetExtension(output.Value.ToString());
            return JsonExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
        }

        return false;
    }

    public async Task<EmitResult> FormatAsync(SymbolRef scopeRef, EmitScope scope, CancellationToken cancellationToken)
    {
        var relativePath = GetOutputRelativePath(scopeRef, scope);
        var outputPath = Path.Combine(_outputDirectory.FullName, relativePath);

        await using var outputWriter = File.CreateText(outputPath);

        await scope
            .AcceptAsync(new WriterAdapter(new JsonWriter(outputWriter)), cancellationToken)
            .ConfigureAwait(false);

        await outputWriter
            .FlushAsync()
            .ConfigureAwait(false);

        return new EmitResult(scopeRef, outputPath, 0, 0);
    }

    private static string GetOutputRelativePath(SymbolRef scopeRef, EmitScope scope)
    {
        if (scope.Attributes.TryGetValue(Attributes.Output, out var output) && !output.Value.IsEmpty)
        {
            return output.Value.ToString();
        }

        return scopeRef.Path[^1].ToString() + JsonExtensions[0];
    }
}
