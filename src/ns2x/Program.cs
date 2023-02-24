using ns2x;
using ns2x.Lexer;
using ns2x.Parser;

var input = File.ReadAllText(args[0]);
var source = new SourceImpl(input.AsMemory());

var lexer = new LexerImpl();
var tokens = lexer.Process(source);

var parser = new ParserImpl();
var semanticModel = parser.Execute(source, tokens);

foreach (var token in tokens)
{
    Console.Write($"[{token.Type:F}");

    if (!token.IsHidden())
    {
        Console.Write(" :: ");
        Console.Write(source.Text(token).ToString());
    }

    Console.Write("]");

    if (token.Type == TokenType.Eol)
        Console.WriteLine();
}