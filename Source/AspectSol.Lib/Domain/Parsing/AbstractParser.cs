using AspectSol.Lib.Domain.AST;
using AspectSol.Lib.Domain.Tokenization;
using AspectSol.Lib.Infra.Enums;
using AspectSol.Lib.Infra.Exceptions;

namespace AspectSol.Lib.Domain.Parsing;

public abstract class AbstractParser : IParser
{
    protected Stack<DslToken> TokenSequence;
    protected DslToken LookaheadFirst;
    protected DslToken LookaheadSecond;

    public abstract AspectNode Parse(List<DslToken> tokens);

    protected void Initialize(List<DslToken> tokens)
    {
        LoadSequenceStack(tokens);
        PrepareLookaheads();
    }

    protected void LoadSequenceStack(IReadOnlyList<DslToken> tokens)
    {
        TokenSequence = new Stack<DslToken>();

        var count = tokens.Count;
        for (var i = count - 1; i >= 0; i--)
        {
            TokenSequence.Push(tokens[i]);
        }
    }

    protected void PrepareLookaheads()
    {
        LookaheadFirst = TokenSequence.Pop();
        LookaheadSecond = TokenSequence.Pop();
    }

    protected DslToken ReadToken(TokenType expectedTokenType)
    {
        if (LookaheadFirst.TokenType != expectedTokenType)
            throw new DslParserException($"Expected [{expectedTokenType.ToString().ToUpper()}] but found: [{LookaheadFirst.Value}]");

        return LookaheadFirst;
    }

    protected void ValidateToken(TokenType expectedTokenType)
    {
        if (LookaheadFirst.TokenType != expectedTokenType)
            throw new DslParserException($"Expected [{expectedTokenType.ToString().ToUpper()}] but found: [{LookaheadFirst.Value}]");
    }

    protected void DiscardToken()
    {
        LookaheadFirst = LookaheadSecond.Clone();
        LookaheadSecond = TokenSequence.Any() ? TokenSequence.Pop() : new DslToken(TokenType.SequenceTerminator, string.Empty);
    }

    protected void DiscardToken(TokenType expectedTokenType)
    {
        if (LookaheadFirst.TokenType != expectedTokenType)
            throw new DslParserException($"Expected [{expectedTokenType.ToString().ToUpper()}] but found: [{LookaheadFirst.Value}]");

        DiscardToken();
    }
}