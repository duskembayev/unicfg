namespace unicfg.Formatters.Writers;

/// <summary>
///     JSON Writer.
/// </summary>
internal class JsonWriter : WriterBase
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="JsonWriter" /> class.
    /// </summary>
    /// <param name="textWriter">The text writer.</param>
    public JsonWriter(TextWriter textWriter) : base(textWriter)
    {
    }

    /// <summary>
    ///     Indicates whether or not the produced document will be written in a compact or pretty fashion.
    /// </summary>
    public bool ProduceTerseOutput { get; init; } = false;

    /// <summary>
    ///     Base Indentation Level.
    ///     This denotes how many indentations are needed for the property in the base object.
    /// </summary>
    protected override int BaseIndentation => 1;

    /// <summary>
    ///     Write JSON start object.
    /// </summary>
    public override void WriteStartObject()
    {
        var previousScope = CurrentScope();

        var currentScope = StartScope(ScopeType.Object);

        if (previousScope != null && previousScope.Type == ScopeType.Array)
        {
            currentScope.IsInArray = true;

            if (previousScope.ObjectCount != 1)
            {
                Writer.Write(WriterConstants.ArrayElementSeparator);
            }

            WriteLine();
            WriteIndentation();
        }

        Writer.Write(WriterConstants.StartObjectScope);

        IncreaseIndentation();
    }

    /// <summary>
    ///     Write JSON end object.
    /// </summary>
    public override void WriteEndObject()
    {
        var currentScope = EndScope(ScopeType.Object);
        if (currentScope.ObjectCount != 0)
        {
            WriteLine();
            DecreaseIndentation();
            WriteIndentation();
        }
        else
        {
            if (!ProduceTerseOutput)
            {
                Writer.Write(WriterConstants.WhiteSpaceForEmptyObject);
            }

            DecreaseIndentation();
        }

        Writer.Write(WriterConstants.EndObjectScope);
    }

    /// <summary>
    ///     Write JSON start array.
    /// </summary>
    public override void WriteStartArray()
    {
        var previousScope = CurrentScope();

        var currentScope = StartScope(ScopeType.Array);

        if (previousScope != null && previousScope.Type == ScopeType.Array)
        {
            currentScope.IsInArray = true;

            if (previousScope.ObjectCount != 1)
            {
                Writer.Write(WriterConstants.ArrayElementSeparator);
            }

            WriteLine();
            WriteIndentation();
        }

        Writer.Write(WriterConstants.StartArrayScope);
        IncreaseIndentation();
    }

    /// <summary>
    ///     Write JSON end array.
    /// </summary>
    public override void WriteEndArray()
    {
        var current = EndScope(ScopeType.Array);
        if (current.ObjectCount != 0)
        {
            WriteLine();
            DecreaseIndentation();
            WriteIndentation();
        }
        else
        {
            Writer.Write(WriterConstants.WhiteSpaceForEmptyArray);
            DecreaseIndentation();
        }

        Writer.Write(WriterConstants.EndArrayScope);
    }

    /// <summary>
    ///     Write property name.
    /// </summary>
    /// <param name="name">The property name.</param>
    /// public override void WritePropertyName(string name)
    public override void WritePropertyName(string name)
    {
        VerifyCanWritePropertyName(name);

        var currentScope = CurrentScope();
        if (currentScope!.ObjectCount != 0)
        {
            Writer.Write(WriterConstants.ObjectMemberSeparator);
        }

        WriteLine();

        currentScope.ObjectCount++;

        WriteIndentation();

        name = name.GetJsonCompatibleString();

        Writer.Write(name);

        Writer.Write(WriterConstants.NameValueSeparator);

        if (!ProduceTerseOutput)
        {
            Writer.Write(WriterConstants.NameValueSeparatorWhiteSpaceSuffix);
        }
    }

    /// <summary>
    ///     Write string value.
    /// </summary>
    /// <param name="value">The string value.</param>
    public override void WriteValue(string value)
    {
        WriteValueSeparator();

        value = value.GetJsonCompatibleString();

        Writer.Write(value);
    }

    /// <summary>
    ///     Write null value.
    /// </summary>
    public override void WriteNull()
    {
        WriteValueSeparator();

        Writer.Write("null");
    }

    /// <summary>
    ///     Writes a separator of a value if it's needed for the next value to be written.
    /// </summary>
    protected override void WriteValueSeparator()
    {
        if (Scopes.Count == 0)
        {
            return;
        }

        var currentScope = Scopes.Peek();

        if (currentScope.Type == ScopeType.Array)
        {
            if (currentScope.ObjectCount != 0)
            {
                Writer.Write(WriterConstants.ArrayElementSeparator);
            }

            WriteLine();
            WriteIndentation();
            currentScope.ObjectCount++;
        }
    }

    /// <summary>
    ///     Writes the content raw value.
    /// </summary>
    public override void WriteRaw(string value)
    {
        WriteValueSeparator();
        Writer.Write(value);
    }

    /// <summary>
    ///     Write the indentation.
    /// </summary>
    public override void WriteIndentation()
    {
        if (ProduceTerseOutput)
        {
            return;
        }

        base.WriteIndentation();
    }

    /// <summary>
    ///     Writes a line terminator to the text string or stream.
    /// </summary>
    private void WriteLine()
    {
        if (ProduceTerseOutput)
        {
            return;
        }

        Writer.WriteLine();
    }
}
