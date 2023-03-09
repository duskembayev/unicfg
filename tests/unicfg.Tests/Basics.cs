using NUnit.Framework;
using unicfg.Evaluation;
using unicfg.Model.Analysis;
using unicfg.Model.Elements;
using unicfg.Model.Primitives;
using unicfg.Model.Sources;
using unicfg.Uni.Lex;
using unicfg.Uni.Tree;

namespace unicfg.Tests;

[TestFixture]
public class Basics
{
    private Document _document;
    private PropertyResolver _propertyResolver;
    private EvaluatorImpl _evaluator;

    private const string Input = @"
message.dot=!!
message.part.1=Hello
message.part.2=World
message.result=${message.part.1} ${message.part.2}${message.dot}";

    [OneTimeSetUp]
    public void Setup()
    {
        var source = Source.Create(Input);
        var diagnostics = new Diagnostics(source);
        var lexer = new LexerImpl(diagnostics);
        var tokens = lexer.Process(source);
        var parser = new ParserImpl(diagnostics);

        _document = parser.Execute(source, tokens);
        _propertyResolver = new PropertyResolver(_document);
        _evaluator = new EvaluatorImpl(_propertyResolver, diagnostics);
    }

    [Test]
    public void EvaluateSimpleProperty()
    {
        var property = _propertyResolver.ResolveProperty(PropertyRef.FromPath("message.dot"));
        Assert.NotNull(property);

        var value = _evaluator.Evaluate(property);
        Assert.AreEqual("!", value);
    }

    [Test]
    public void EvaluateRefProperty()
    {
        var property = _propertyResolver.ResolveProperty(PropertyRef.FromPath("message.result"));
        Assert.NotNull(property);

        var value = _evaluator.Evaluate(property);
        Assert.AreEqual("Hello World!", value);
    }
}