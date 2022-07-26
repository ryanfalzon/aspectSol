using AspectSol.Lib.Domain.Tokenizer;
using AspectSol.Lib.Infra.Exceptions;

namespace AspectSol.Lib.Domain.Parser;

public abstract class AbstractParser
{
    protected DslToken LookaheadFirst;
    protected DslToken LookaheadSecond;

    private Stack<DslToken> _tokenSequence;

    protected void Initialize(List<DslToken> tokens)
    {
        LoadSequenceStack(tokens);
        PrepareLookaheads();
    }

    /// <summary>
    /// Ensures that the type of the first lookahead matches the 'expectedTokenType' parameter
    /// </summary>
    /// <param name="expectedTokenType"></param>
    /// <param name="detailedMessage"></param>
    /// <exception>
    ///     <cref>ParserException</cref>
    /// </exception>
    protected void ValidateToken(TokenType expectedTokenType, string detailedMessage)
    {
        if (LookaheadFirst.TokenType != expectedTokenType) throw ParserException(expectedTokenType, detailedMessage);
    }

    /// <summary>
    /// Replaces the token in the first lookahead with that of the second lookahead
    /// </summary>
    protected void DiscardToken()
    {
        LookaheadFirst  = LookaheadSecond.Clone();
        LookaheadSecond = _tokenSequence.Any() ? _tokenSequence.Pop() : new DslToken(TokenType.SequenceTerminator, string.Empty, 0, 0);
    }

    /// <summary>
    /// Replaces the token in the first lookahead with that of the second lookahead if the type of the first lookahead matches the 'expectedTokenType' parameter
    /// </summary>
    /// <param name="expectedTokenType"></param>
    /// <param name="detailedMessage"></param>
    /// <exception>
    ///     <cref>ParserException</cref>
    /// </exception>
    protected void DiscardToken(TokenType expectedTokenType, string detailedMessage)
    {
        if (LookaheadFirst.TokenType != expectedTokenType) throw ParserException(expectedTokenType, detailedMessage);
        DiscardToken();
    }

    /// <summary>
    /// Creates a new instance of 'ParserException' with a short message in the template of 'Expected [] but found []'
    /// </summary>
    /// <param name="expectedType"></param>
    /// <param name="detailedMessage"></param>
    /// <returns></returns>
    private ParserException ParserException(TokenType expectedType, string detailedMessage) =>
        new ParserException(
            detailedMessage: detailedMessage,
            shortMessage: $"Expected [{expectedType.ToString().ToUpper()}] but found [{LookaheadFirst.TokenType.ToString().ToUpper()}]",
            lineNumber: LookaheadFirst.LineNumber,
            position: LookaheadFirst.Position);

    /// <summary>
    /// Creates a new instance of 'ParserException' with a short message in the template of 'Found []'
    /// </summary>
    /// <param name="detailedMessage"></param>
    /// <returns></returns>
    protected ParserException ParserException(string detailedMessage) =>
        new ParserException(
            detailedMessage: detailedMessage,
            shortMessage: $"Found [{LookaheadFirst.TokenType.ToString().ToUpper()}]",
            lineNumber: LookaheadFirst.LineNumber,
            position: LookaheadFirst.Position);

    private void LoadSequenceStack(IReadOnlyList<DslToken> tokens)
    {
        _tokenSequence = new Stack<DslToken>();

        var count = tokens.Count;
        for (var i = count - 1; i >= 0; i--)
        {
            _tokenSequence.Push(tokens[i]);
        }
    }

    private void PrepareLookaheads()
    {
        LookaheadFirst  = _tokenSequence.Pop();
        LookaheadSecond = _tokenSequence.Pop();
    }
}