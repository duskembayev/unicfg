using System.Collections.Immutable;
using unicfg;
using unicfg.Base.Analysis;
using unicfg.Base.Elements;
using unicfg.Base.Environment;
using unicfg.Base.Extensions;
using unicfg.Base.Primitives;
using unicfg.Base.Sources;
using unicfg.Evaluation;
using unicfg.Uni.Lex;
using unicfg.Uni.Tree;

var source = Source.FromFile(args[0]);
var diagnostics = new Diagnostics().WithSource(source);

var lexer = new LexerImpl(diagnostics);
var tokens = lexer.Process(source);

var parser = new ParserImpl(diagnostics, new CurrentProcess());
var document = parser.Execute(source, tokens);

var propertyResolver = new PropertyResolver(document);
var evaluator = new EvaluatorImpl(propertyResolver, diagnostics.WithSource(source));
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
    public sealed class ValueEvaluator : AbstractElementVisitor
    {
        private readonly EvaluatorImpl _evaluator;
        private readonly TextWriter _writer;

        public ValueEvaluator(EvaluatorImpl evaluator, TextWriter writer)
        {
            _evaluator = evaluator;
            _writer = writer;
        }

        public override void Visit(UniProperty property)
        {
            _writer.WriteLine("{0}={1}", property.ToDisplayName(), _evaluator.Evaluate(property));
        }

        public override void Visit(UniAttribute attribute)
        {
            _writer.WriteLine("{0}={1}", attribute.ToDisplayName(), _evaluator.Evaluate(attribute));
        }
    }
}