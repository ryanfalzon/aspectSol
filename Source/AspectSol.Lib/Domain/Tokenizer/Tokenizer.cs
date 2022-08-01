using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace AspectSol.Lib.Domain.Tokenizer;

public class Tokenizer : AbstractTokenizer, ITokenizer
{
    public Tokenizer(ILogger<Tokenizer> logger) : base(logger)
    {
        TokenDefinitions = new List<TokenDefinition>
        {
            // AspectSol Keywords
            new(TokenType.Scope,new Regex("^(?s)(¬{{1}.*?}¬{1})")),
            new(TokenType.CallTo, new Regex("^call-to")),
            new(TokenType.ExecutionOf, new Regex("^execution-of")),
            new(TokenType.TaggedWith, new Regex("^tagged-with")),
            new(TokenType.ImplementingInterface, new Regex("^implementing-interface")),
            new(TokenType.ReturningTypes, new Regex("^returning-types")),
            new(TokenType.InInterface, new Regex("^in-interface")),
            new(TokenType.NotInInterface, new Regex("^not-in-interface")),
            new(TokenType.OriginatingFrom, new Regex("^originating-from")),
            new(TokenType.AddToDeclaration, new Regex("^add-to-declaration")),
            new(TokenType.UpdateDefinition, new Regex("^update-definition")),
            new(TokenType.Public, new Regex("^public")),
            new(TokenType.Private, new Regex("^private")),
            new(TokenType.Internal, new Regex("^internal")),
            new(TokenType.External, new Regex("^external")),
            new(TokenType.Before, new Regex("^before")),
            new(TokenType.After, new Regex("^after")),
            new(TokenType.Pure, new Regex("^pure")),
            new(TokenType.View, new Regex("^view")),
            new(TokenType.Get, new Regex("^get\\s")),
            new(TokenType.Set, new Regex("^set")),
            new(TokenType.Aspect, new Regex("^aspect")),
            
            // AspectSol Conditions
            new(TokenType.NotSymbol, new Regex("^\\!")),
            new(TokenType.AndSymbol, new Regex("^\\&")),
            new(TokenType.OrSymbol, new Regex("^\\|")),
            
            // AspectSol Symbols
            new(TokenType.Wildcard, new Regex("^\\*")),
            new(TokenType.OpenParenthesis, new Regex("^\\(")),
            new(TokenType.CloseParenthesis, new Regex("^\\)")),
            new(TokenType.Comma, new Regex("^\\,")),
            new(TokenType.FullStop, new Regex("^\\.")),
            new(TokenType.DoubleColon, new Regex("^[:]{2}")),
            new(TokenType.OpenDoubleSquareBrackets, new Regex("^[[]{2}")),
            new(TokenType.CloseDoubleSquareBrackets, new Regex("^[]]{2}")),
            new(TokenType.OpenSquareBrackets, new Regex("^\\[")),
            new(TokenType.CloseSquareBrackets, new Regex("^\\]")),
            new(TokenType.OpenScope, new Regex("^\\{")),
            new(TokenType.CloseScope, new Regex("^\\}")),
            
            // AspectSol Other Items
            new(TokenType.Number, new Regex("^[0-9]+")),
            new(TokenType.StringValue, new Regex("^([\"]{1}|[']{1})[a-zA-Z0-9]*([\"]{1}|[']{1})")),
            new(TokenType.ArbitraryWord, new Regex("^[a-zA-Z0-9]+"))
        };
    }
}