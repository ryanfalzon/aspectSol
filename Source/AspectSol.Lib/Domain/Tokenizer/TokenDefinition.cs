using System.Text.RegularExpressions;

namespace AspectSol.Lib.Domain.Tokenizer;

public class TokenDefinition
{
    private readonly Regex _regex;
    private readonly TokenType _returnsToken;

    public TokenDefinition(TokenType returnsToken, string regexPattern)
    {
        _regex        = new Regex(regexPattern);
        _returnsToken = returnsToken;
    }

    public TokenDefinition(TokenType returnsToken, Regex regex)
    {
        _regex        = regex;
        _returnsToken = returnsToken;
    }

    public TokenMatch Match(string inputString)
    {
        var match = _regex.Match(inputString);

        if (!match.Success) return new TokenMatch { IsMatch = false };

        var remainingText = match.Length != inputString.Length ? inputString[match.Length..] : string.Empty;
        return new TokenMatch
        {
            IsMatch       = true,
            RemainingText = remainingText,
            TokenType     = _returnsToken,
            Value         = match.Value
        };
    }
}