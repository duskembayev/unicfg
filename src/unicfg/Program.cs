using System.Collections.Immutable;
using unicfg;
using unicfg.Evaluator;
using unicfg.Lexer;
using unicfg.Model;
using unicfg.Model.Analysis;
using unicfg.Model.Extensions;
using unicfg.Model.Semantic;
using unicfg.Parser;
using Attribute = unicfg.Model.Semantic.Attribute;

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

        if (!token.IsEndOfLine())
        {
            Console.Write(" :: ");
            Console.Write(iSource.GetText(token.RawRange).ToString());
        }

        Console.Write("]");

        if (token.Type == TokenType.Eol)
            Console.WriteLine();
    }
}

namespace unicfg
{
    public sealed class ValueEvaluator : Walker
    {
        private readonly EvaluatorImpl _evaluator;
        private readonly TextWriter _writer;

        public ValueEvaluator(EvaluatorImpl evaluator, TextWriter writer)
        {
            _evaluator = evaluator;
            _writer = writer;
        }

        public override void Visit(Property property)
        {
            _writer.WriteLine("{0}={1}", property.ToDisplayName(), _evaluator.Evaluate(property));
        }

        public override void Visit(Attribute attribute)
        {
            _writer.WriteLine("{0}={1}", attribute.ToDisplayName(), _evaluator.Evaluate(attribute));
        }
    }
}