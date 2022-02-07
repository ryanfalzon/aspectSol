using AspectSol.Lib.Infra.Enums;
using AspectSol.Lib.Infra.Exceptions;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace AspectSol.Lib.Domain.Tokenization;

public abstract class AbstractTokenizer : ITokenizer
{
    protected readonly ILogger<Tokenizer> Logger;
    protected List<TokenDefinition> TokenDefinitions;

    public abstract List<DslToken> Tokenize(string source, bool verboseLogging = false);

    protected AbstractTokenizer(ILogger<Tokenizer> logger)
    {
        Logger = logger;
    }

    protected TokenMatch FindMatch(string source)
    {
        foreach (var tokenDefinition in TokenDefinitions)
        {
            var match = tokenDefinition.Match(source);
            if (match.IsMatch) return match;
        }

        return new() { IsMatch = false };
    }

    protected static bool IsWhitespace(string source)
    {
        return Regex.IsMatch(source, "^\\s+");
    }

    protected static TokenMatch CreateInvalidTokenMatch(string lqlText)
    {
        var match = Regex.Match(lqlText, "(^\\S+\\s)|^\\S+");
        if (match.Success)
        {
            return new()
            {
                IsMatch = true,
                RemainingText = lqlText[match.Length..],
                TokenType = TokenType.Invalid,
                Value = match.Value.Trim()
            };
        }

        throw new DslParserException("Failed to generate invalid token");
    }
}