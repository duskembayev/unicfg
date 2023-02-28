using System.Collections.Immutable;
using ns2x;
using ns2x.Evaluator;
using ns2x.Lexer;
using ns2x.Model.Analysis;
using ns2x.Model.Semantic;
using ns2x.Parser;
using Attribute = ns2x.Model.Semantic.Attribute;

var input = File.ReadAllText(args[0]);
var source = new SourceImpl(input.AsMemory());
var diagnostics = new Diagnostics(source);

var lexer = new LexerImpl(diagnostics);
var tokens = lexer.Process(source);

var parser = new ParserImpl(diagnostics);
var document = parser.Execute(source, tokens);

var propertyResolver = new PropertyResolver(document);
var evaluator = new EvaluatorImpl(propertyResolver, diagnostics);
document.Accept(new ValueEvaluator(evaluator, Console.Out));


//PrintTokens(tokens, source);

foreach (var diagnostic in diagnostics)
    Console.WriteLine(diagnostic.ToString());

void PrintTokens(ImmutableArray<Token> iTokens, ISource iSource)
{
    foreach (var token in iTokens)
    {
        Console.Write($"[{token.Type:F}");

        if (!token.IsHidden())
        {
            Console.Write(" :: ");
            Console.Write(iSource.GetText(token.Range).ToString());
        }

        Console.Write("]");

        if (token.Type == TokenType.Eol)
            Console.WriteLine();
    }
}

public sealed class ValueEvaluator : Walker
{
    private readonly EvaluatorImpl _evaluator;
    private readonly TextWriter _writer;
    private Stack<StringRef> _path;

    public ValueEvaluator(EvaluatorImpl evaluator, TextWriter writer)
    {
        _evaluator = evaluator;
        _writer = writer;
        _path = new Stack<StringRef>();
    }

    public override void Visit(Namespace ns)
    {
        if (!ns.Name.Equals(StringRef.Empty))
            _path.Push(ns.Name);

        base.Visit(ns);

        if (!ns.Name.Equals(StringRef.Empty))
            _path.Pop();
    }

    public override void Visit(Property property)
    {
        _path.Push(property.Name);
        var path = string.Join('.', _path.Reverse());
        var value = _evaluator.Evaluate(property);
        _writer.WriteLine("{0}={1}", path, value);
        base.Visit(property);
        _path.Pop();
    }

    public override void Visit(Attribute attribute)
    {
        var path = string.Join('.', _path.Reverse());
        var value = _evaluator.Evaluate(attribute);
        _writer.WriteLine("{0}[{2}]={1}", path, value, attribute.Name);
    }
}