namespace unicfg.Evaluation;

public readonly record struct DocumentFormat(string DefaultExtension)
{
    public static readonly DocumentFormat Uni = new(".uni");
    public static readonly DocumentFormat Yaml = new(".yaml");
    public static readonly DocumentFormat Json = new(".json");
    public static readonly DocumentFormat Xml = new(".xml");
    public static readonly DocumentFormat Ini = new(".ini");
    public static readonly DocumentFormat Properties = new(".properties");
}