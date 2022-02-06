using AspectSol.Lib.Infra.Enums;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace AspectSol.Lib.Domain.Tokenization;

public class Tokenizer : AbstractTokenizer
{
    public Tokenizer(ILogger<Tokenizer> logger) : base(logger)
    {
        TokenDefinitions = new()
        {
            new(TokenType.Before, "^before"),
            new(TokenType.After, "^after"),
            new(TokenType.CallTo, "^call-to"),
            new(TokenType.ExecutionOf, "^execution-of"),
            new(TokenType.OpenParenthesis, "^\\("),
            new(TokenType.CloseParenthesis, "^\\)"),
            new(TokenType.Comma, "^,"),
            new(TokenType.FullStop, "^."),
            new(TokenType.DoubleColon, "^::"),
            new(TokenType.OpenDoubleSquareBrackets, "^[["),
            new(TokenType.CloseDoubleSquareBrackets, "^]]"),
            new(TokenType.Wildcard, "^*"),
            new(TokenType.TaggedWith, "^tagged-with"),
            new(TokenType.ImplementingInterface, "^implementing-interface"),
            new(TokenType.NotSymbol, "^!"),
            new(TokenType.AndSymbol, "^&"),
            new(TokenType.OrSymbol, "^|"),
            new(TokenType.Public, "^public"),
            new(TokenType.Private, "^private"),
            new(TokenType.Internal, "^internal"),
            new(TokenType.External, "^external"),
            new(TokenType.Pure, "^pure"),
            new(TokenType.View, "^view"),
            new(TokenType.ReturningTypes, "^returning-types"),
            new(TokenType.InInterface, "^in-interface"),
            new(TokenType.NotInInterface, "^not-in-interface"),
            new(TokenType.Get, "^get"),
            new(TokenType.Set, "^set"),
            new(TokenType.OpenSquareBrackets, "^["),
            new(TokenType.CloseSquareBrackets, "^]"),
            new(TokenType.OriginatingFrom, "^originating-from"),
            new(TokenType.AddToDeclaration, "^add-to-declaration"),
            new(TokenType.UpdateDefinition, "^update-definition"),
            new(TokenType.StringValue, "^'[^']*'"),
            new(TokenType.Number, "^\\d+"),
            new(TokenType.OpenScope, "^{"),
            new(TokenType.CloseScope, "^}"),
            new(TokenType.Aspect, "^aspect")
        };
    }

    public override List<DslToken> Tokenize(string source, bool verboseLogging = false)
    {
        var tokens = new List<DslToken>();

        var remainingText = source;

        while (!string.IsNullOrWhiteSpace(remainingText))
        {
            var match = FindMatch(remainingText);
            if (match.IsMatch)
            {
                tokens.Add(new(match.TokenType, match.Value));
                remainingText = match.RemainingText;
            }
            else
            {
                if (IsWhitespace(remainingText))
                {
                    remainingText = remainingText[1..];
                }
                else
                {
                    var invalidTokenMatch = CreateInvalidTokenMatch(remainingText);
                    tokens.Add(new(invalidTokenMatch.TokenType, invalidTokenMatch.Value));
                    remainingText = invalidTokenMatch.RemainingText;
                }
            }
        }

        tokens.Add(new(TokenType.SequenceTerminator, string.Empty));

        return tokens;
    }
}