using AspectSol.Compiler.Infra.Enums;
using System.Text.RegularExpressions;

namespace AspectSol.Compiler.Infra.Models
{
    public class TokenDefinition
    {
        private readonly Regex regex;
        private readonly TokenType returnableToken;

        public TokenDefinition(TokenType tokenType, string regexPattern)
        {
            regex = new Regex(regexPattern);
            returnableToken = tokenType;
        }
        
        public TokenMatch Match(string inputString)
        {
            var tokenMatch = new TokenMatch();

            var match = regex.Match(inputString);
            if (match.Success)
            {
                tokenMatch.IsMatch = true;
                tokenMatch.RemainingText = match.Length != inputString.Length ? inputString[match.Length..] : string.Empty;
                tokenMatch.Token = new Token(returnableToken, match.Value);
            }

            return tokenMatch;
        }
    }
}