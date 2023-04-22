using NUnit.Framework;
using unicfg.Base.Analysis;
using unicfg.Base.Inputs;
using unicfg.Base.Primitives;
using unicfg.Base.SyntaxTree;
using unicfg.Uni.Lex;
using unicfg.Uni.Tree;

namespace unicfg.Tests;

[TestFixture]
public class Attributes
{
    [OneTimeSetUp]
    public void Setup()
    {
        var source = Source.Create(Input);
        var diagnostics = new Diagnostics().WithSource(source);
        var lexer = new LexerImpl(diagnostics);
        var tokens = lexer.Process(source);
        var parser = new ParserImpl(diagnostics, new CurrentProcess());

        _document = parser.Execute(source, tokens);
        _propertyResolver = new PropertyResolver(_document);
        _evaluator = new EvaluatorImpl(_propertyResolver, diagnostics);
    }

    private Document _document;
    private PropertyResolver _propertyResolver;
    private EvaluatorImpl _evaluator;

    private const string Input = @"
subjectColor=white
picture[border]=10
picture.background[color]=black
picture.background=night
picture.subject[color]=${subjectColor}
picture.subject=moon";

    [Test]
    public void ResolveNamespaceAttribute()
    {
        var group = _document.RootScope.Scopes.Single(n => n.Name.Equals("picture"));
        var attribute = group.Attributes.Single(a => a.Name.Equals("border"));
        var value = _evaluator.Evaluate(attribute);

        Assert.AreEqual("10", value);
    }

    [Test]
    public void ResolvePropertyAttribute()
    {
        var property = _propertyResolver.ResolveProperty(SymbolRef.FromPath("picture.background"));
        Assert.NotNull(property);

        var attribute = property.Attributes.Single(a => a.Name.Equals("color"));
        var value = _evaluator.Evaluate(attribute);

        Assert.AreEqual("black", value);
    }

    [Test]
    public void ResolvePropertyAttributeWithRef()
    {
        var property = _propertyResolver.ResolveProperty(SymbolRef.FromPath("picture.subject"));
        Assert.NotNull(property);

        var attribute = property.Attributes.Single(a => a.Name.Equals("color"));
        var value = _evaluator.Evaluate(attribute);

        Assert.AreEqual("white", value);
    }
}
