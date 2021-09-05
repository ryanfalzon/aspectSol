namespace AspectSol.Compiler.Infra.Enums
{
    public enum TokenType
    {
        KeywordBefore,

        KeywordAfter,

        KeywordCallTo,

        KeywordExecutionOf,

        KeywordReturning,

        KeywordInInterface,

        KeywordNotInInterface,

        KeywordGet,

        KeywordSet,

        KeywordTaggedWith,

        KeywordOriginatingFrom,

        VariableType,

        VariableName,

        VariableKey,

        OperatorAnd,

        OperatorOr,

        OperatorNot,

        CharacterDot,

        CharacterOpenMapping,

        CharacterCloseMapping,

        StringValue,

        Number,

        SequenceTerminator,

        StatementTerminator
    }
}