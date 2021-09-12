using AspectSol.Compiler.Domain;
using AspectSol.Compiler.Infra.Models;
using AspectSol.Compiler.Infra.Enums;
using System.Collections.Generic;
using System.IO;

namespace AspectSol.Compiler.App.Processors
{
    public class Tokenizer : ITokenizer
    {
        private readonly List<TokenDefinition> tokenDefinitions;

        public Tokenizer()
        {
            tokenDefinitions = new List<TokenDefinition>
            {
                new TokenDefinition(TokenType.KeywordBefore, ""),
                new TokenDefinition(TokenType.KeywordAfter, ""),
                new TokenDefinition(TokenType.KeywordCallTo, ""),
                new TokenDefinition(TokenType.KeywordExecutionOf, ""),
                new TokenDefinition(TokenType.KeywordReturning, ""),
                new TokenDefinition(TokenType.KeywordInInterface, ""),
                new TokenDefinition(TokenType.KeywordNotInInterface, ""),
                new TokenDefinition(TokenType.KeywordGet, ""),
                new TokenDefinition(TokenType.KeywordSet, ""),
                new TokenDefinition(TokenType.KeywordTaggedWith, ""),
                new TokenDefinition(TokenType.KeywordOriginatingFrom, ""),
                new TokenDefinition(TokenType.VariableType, ""),
                new TokenDefinition(TokenType.VariableName, ""),
                new TokenDefinition(TokenType.VariableKey, ""),
                new TokenDefinition(TokenType.OperatorAnd, ""),
                new TokenDefinition(TokenType.OperatorOr, ""),
                new TokenDefinition(TokenType.OperatorNot, ""),
                new TokenDefinition(TokenType.CharacterDot, ""),
                new TokenDefinition(TokenType.CharacterOpenMapping, ""),
                new TokenDefinition(TokenType.CharacterCloseMapping, ""),
                new TokenDefinition(TokenType.StringValue, ""),
                new TokenDefinition(TokenType.Number, ""),
                new TokenDefinition(TokenType.SequenceTerminator, ""),
                new TokenDefinition(TokenType.StatementTerminator, "")
            };
        }

        public List<Token> Start(string filepath)
        {
            var tokens = new List<Token>();
            var remainingText = File.ReadAllText(filepath);

            while (!string.IsNullOrWhiteSpace(remainingText))
            {
                var match = FindMatch(remainingText);
                if (match.IsMatch)
                {
                    tokens.Add(match.Token);
                    remainingText = match.RemainingText;
                }
                else
                {
                    //if (remainingText.IsWhitespace())
                    //{
                    //    remainingText = remainingText[1..];
                    //}
                    //else
                    //{

                    //}
                }
            }

            tokens.Add(new Token(TokenType.SequenceTerminator, string.Empty));
            return tokens;
        }

        private TokenMatch FindMatch(string text)
        {
            foreach (var tokenDefinition in tokenDefinitions)
            {
                var match = tokenDefinition.Match(text);
                if (match.IsMatch)
                {
                    return match;
                }
            }

            return new TokenMatch() { IsMatch = false };
        }
    }
}