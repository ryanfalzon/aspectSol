using AspectSol.Lib.Domain.Tokenizer;

namespace AspectSol.Lib.Infra.Extensions;

public static class DslTokenExtensions
{
    public static bool IsDefinitionSelector(this DslToken token)
    {
        return token.TokenType is TokenType.Wildcard or TokenType.OpenDoubleSquareBrackets or TokenType.Address or TokenType.ArbitraryWord;
    }

    public static bool IsVariableSelector(this DslToken token)
    {
        return token.TokenType is TokenType.Get or TokenType.Set;
    }

    public static bool IsModifierOrVisibility(this DslToken token)
    {
        return token.TokenType is TokenType.StringValue or TokenType.Public or TokenType.Private or TokenType.Internal or TokenType.External or TokenType.Pure
            or TokenType.View;
    }

    public static bool IsModificationStatement(this DslToken token)
    {
        return token.TokenType is TokenType.AddToDeclaration or TokenType.UpdateDefinition;
    }

    public static bool IsAppendStatement(this DslToken token)
    {
        return token.TokenType is TokenType.Before or TokenType.After;
    }
}