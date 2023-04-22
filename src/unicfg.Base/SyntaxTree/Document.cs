namespace unicfg.Base.SyntaxTree;

public sealed class Document : IElement
{
    private ScopeSymbol? _rootGroup;

    internal Document(string baseDirectory, string? location)
    {
        BaseDirectory = baseDirectory;
        Location = location;
    }

    public string? Location { get; }
    public string BaseDirectory { get; }

    public ScopeSymbol RootScope
    {
        get => _rootGroup ?? throw new InvalidOperationException();
        internal set => _rootGroup = value;
    }

    public T Accept<T>(IElementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}
