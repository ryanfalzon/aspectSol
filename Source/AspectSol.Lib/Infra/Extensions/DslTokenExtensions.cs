using AspectSol.Lib.Domain.Tokenization;
using AspectSol.Lib.Infra.Enums;
using AspectSol.Lib.Infra.Exceptions;

namespace AspectSol.Lib.Infra.Extensions;

public static class DslTokenExtensions
{
    public static bool IsDefinitionSelector(this DslToken token)
    {
        return token.TokenType is TokenType.StringValue or TokenType.Wildcard or TokenType.OpenDoubleSquareBrackets;
    }

    public static bool IsVariableSelector(this DslToken token)
    {
        return token.TokenType is TokenType.Get or TokenType.Set;
    }

    public static bool IsModifierOrVisibility(this DslToken token)
    {
        return token.TokenType is TokenType.StringValue or TokenType.Public or TokenType.Private or TokenType.Internal or TokenType.External or TokenType.Pure or TokenType.View;
    }

    public static bool IsModificationStatement(this DslToken token)
    {
        return token.TokenType is TokenType.AddToDeclaration or TokenType.UpdateDefinition;
    }

    public static bool IsAppendStatement(this DslToken token)
    {
        return token.TokenType is TokenType.Before or TokenType.After;
    }

    public static bool IsModificationType(this DslToken token)
    {
        return token.TokenType is TokenType.AddToDeclaration or TokenType.UpdateDefinition;
    }

    public static Placement GetPlacement(this DslToken token)
    {
        return token.TokenType switch
        {
            TokenType.Before => Placement.Before,
            TokenType.After => Placement.After,
            _ => throw new DslParserException("")
        };
    }

    public static Location GetLocation(this DslToken token)
    {
        return token.TokenType switch
        {
            TokenType.CallTo => Location.CallTo,
            TokenType.ExecutionOf => Location.ExecutionOf,
            _ => throw new DslParserException("")
        };
    }

    public static VariableAccess GetVariableAccess(this DslToken token)
    {
        return token.TokenType switch
        {
            TokenType.Get => VariableAccess.Get,
            TokenType.Set => VariableAccess.Set,
            _ => throw new DslParserException("")
        };
    }

    public static VariableVisibility GetVariableVisibility(this DslToken token)
    {
        return token.TokenType switch
        {
            TokenType.Public => VariableVisibility.Public,
            TokenType.Private => VariableVisibility.Private,
            TokenType.Internal => VariableVisibility.Internal,
            _ => throw new DslParserException("")
        };
    }

    public static ModificationType GetModificationType(this DslToken token)
    {
        return token.TokenType switch
        {
            TokenType.AddToDeclaration => ModificationType.AddToDeceleration,
            TokenType.UpdateDefinition => ModificationType.UpdateDefinition,
            _ => throw new DslParserException("")
        };
    }
}