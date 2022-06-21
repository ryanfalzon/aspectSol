using System.Text.RegularExpressions;
using AspectSol.Lib.Infra.Enums;
using Microsoft.Extensions.Logging;

namespace AspectSol.Lib.Domain.Tokenization;

public class Tokenizer : AbstractTokenizer, ITokenizer
{
    public Tokenizer(ILogger logger) : base(logger)
    {
        TokenDefinitions = new List<TokenDefinition>
        {
            // AspectSol Keywords
            new(TokenType.Before, new Regex("^before")),
            new(TokenType.After, new Regex("^after")),
            new(TokenType.CallTo, new Regex("^call-to")),
            new(TokenType.ExecutionOf, new Regex("^execution-of")),
            new(TokenType.TaggedWith, new Regex("^tagged-with")),
            new(TokenType.ImplementingInterface, new Regex("^implementing-interface")),
            new(TokenType.Public, new Regex("^public")),
            new(TokenType.Private, new Regex("^private")),
            new(TokenType.Internal, new Regex("^internal")),
            new(TokenType.External, new Regex("^external")),
            new(TokenType.Pure, new Regex("^pure")),
            new(TokenType.View, new Regex("^view")),
            new(TokenType.ReturningTypes, new Regex("^returning-types")),
            new(TokenType.InInterface, new Regex("^in-interface")),
            new(TokenType.NotInInterface, new Regex("^not-in-interface")),
            new(TokenType.Get, new Regex("^get")),
            new(TokenType.Set, new Regex("^set")),
            new(TokenType.OriginatingFrom, new Regex("^originating-from")),
            new(TokenType.AddToDeclaration, new Regex("^add-to-declaration")),
            new(TokenType.UpdateDefinition, new Regex("^update-definition")),
            new(TokenType.Aspect, new Regex("^aspect")),
            
            // AspectSol Conditions
            new(TokenType.NotSymbol, "^!"),
            new(TokenType.AndSymbol, "^&"),
            new(TokenType.OrSymbol, "^|"),
            
            // AspectSol Symbols
            new(TokenType.Wildcard, "^*"),
            new(TokenType.OpenParenthesis, "^\\("),
            new(TokenType.CloseParenthesis, "^\\)"),
            new(TokenType.Comma, "^,"),
            new(TokenType.FullStop, "^."),
            new(TokenType.DoubleColon, "^::"),
            new(TokenType.OpenDoubleSquareBrackets, "^[["),
            new(TokenType.CloseDoubleSquareBrackets, "^]]"),
            new(TokenType.OpenSquareBrackets, "^["),
            new(TokenType.CloseSquareBrackets, "^]"),
            new(TokenType.StringValue, "^'[^']*'"),
            new(TokenType.Number, "^\\d+"),
            new(TokenType.OpenScope, "^{"),
            new(TokenType.CloseScope, "^}"),
            
            // AspectSol Other Items
            new(TokenType.Number, new Regex("^[0-9]+")),
            new(TokenType.StringValue, new Regex("^([\"]{1}|[']{1})[a-zA-Z0-9]*([\"]{1}|[']{1})")),
        };
    }
}