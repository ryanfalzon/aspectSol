using AspectSol.Lib.Domain.Parsing;
using AspectSol.Lib.Domain.Tokenization;
using Microsoft.Extensions.Logging;

namespace AspectSol.Lib.App;

public class AppService
{
    private readonly ILogger<AppService> _logger;
    private readonly ITokenizer _tokenizer;
    private readonly IParser _parser;

    public AppService(ILogger<AppService> logger, ITokenizer tokenizer, IParser parser)
    {
        _logger = logger;
        _tokenizer = tokenizer;
        _parser = parser;
    }

    public async Task Execute(string aspectSolFilePath, string smartContractFilePath, string outputFilePath)
    {
        var aspectSolFileContents = await File.ReadAllTextAsync(aspectSolFilePath);

        var tokenizationResult = _tokenizer.Tokenize(aspectSolFileContents);
        var parsingResult = _parser.Parse(tokenizationResult);

        var smartContractFileContents = await File.ReadAllTextAsync(smartContractFilePath);
    }
}