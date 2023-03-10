﻿using unicfg.Base.Primitives;

namespace unicfg;

public sealed class DocumentOutput
{
    public DocumentOutput(string baseDirectory)
    {
        BaseDirectory = baseDirectory;
    }

    public string BaseDirectory { get; }

    public SymbolRef LinkedPropertyGroup { get; init; }
}