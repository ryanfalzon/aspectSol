using AspectSol.Lib.Domain.Interpreter;
using AspectSol.Lib.Domain.Parser;
using AspectSol.Lib.Domain.Tokenizer;

namespace AspectSol.Lib.App;

public class AppService
{
    private readonly ITokenizer _tokenizer;
    private readonly IParser _parser;
    private readonly IInterpreter _interpreter;

    public AppService(ITokenizer tokenizer, IParser parser, IInterpreter interpreter)
    {
        _tokenizer   = tokenizer;
        _parser      = parser;
        _interpreter = interpreter;
    }

    public string Execute(string script, string smartContract)
    {
        try
        {
            var tokens = _tokenizer.Tokenize(script);
            var ast = _parser.Parse(tokens);
            var response = _interpreter.Interpret(ast, smartContract).GetAwaiter().GetResult();

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return string.Empty;
        }
    }
}