using AspectSol.Lib.Infra.Enums;
using System.Text.RegularExpressions;

namespace AspectSol.Lib.Domain.Tokenization;

public class TokenDefinition
{
    private readonly Regex _regex;
    private readonly TokenType _returnsToken;

    public TokenDefinition(TokenType returnsToken, string regexPattern)
    {
        _regex = new(regexPattern, RegexOptions.IgnoreCase);
        _returnsToken = returnsToken;
    }

    public TokenMatch Match(string inputString)
    {
        var match = _regex.Match(inputString);

        if (!match.Success) return new() { IsMatch = false };

        var remainingText = match.Length != inputString.Length ? inputString[match.Length..] : string.Empty;
        return new()
        {
            IsMatch = true,
            RemainingText = remainingText,
            TokenType = _returnsToken,
            Value = match.Value
        };
    }
}