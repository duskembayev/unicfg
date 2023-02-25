using System.Collections.Immutable;
using ns2x;
using ns2x.Lexer;using ns2x.Model.Analysis;
using ns2x.Parser;

var input = File.ReadAllText(args[0]);
var source = new SourceImpl(input.AsMemory());
var diagnostics = new Diagnostics(source);

var lexer = new LexerImpl(diagnostics);
var tokens = lexer.Process(source);

var parser = new ParserImpl(diagnostics);
var semanticModel = parser.Execute(source, tokens);

//PrintTokens(tokens, source);

foreach (var diagnostic in diagnostics)
{
    Console.WriteLine(diagnostic.ToString());
}

void PrintTokens(ImmutableArray<Token> iTokens, ISource iSource)
{
    foreach (var token in iTokens)
    {
        Console.Write($"[{token.Type:F}");

        if (!token.IsHidden())
        {
            Console.Write(" :: ");
            Console.Write(iSource.Text(token).ToString());
        }

        Console.Write("]");

        if (token.Type == TokenType.Eol)
            Console.WriteLine();
    }
}