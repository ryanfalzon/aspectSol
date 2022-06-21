using AspectSol.Lib.Infra.Exceptions;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace AspectSol.Lib.Domain.Tokenization;

public abstract class AbstractTokenizer
{
    protected List<TokenDefinition> TokenDefinitions;
    private readonly ILogger<AbstractTokenizer> _logger;
    private readonly Regex _lineBreakRegex;

    protected AbstractTokenizer(ILogger<AbstractTokenizer> logger)
    {
        _logger         = logger;
        _lineBreakRegex = new Regex("^(\\s?\r\n|\\s?\r|\\s?\n)");
    }

    public List<DslToken> Tokenize(string source)
    {
        _logger.LogTrace($"|| {nameof(Tokenize)} | Tokenizing script: '{source}'");

        var tokens = new List<DslToken>();

        var lineNumber = _lineBreakRegex.Matches(source).Count + 1;
        var position = 1;
        var remainingText = source.Trim();
        var startNewLine = false;

        while (!string.IsNullOrWhiteSpace(remainingText))
        {
            var match = FindMatch(remainingText);

            var lineBreaksInRemainingText = _lineBreakRegex.Matches(match.RemainingText);

            if (match.IsMatch)
            {
                tokens.Add(new DslToken(match.TokenType, match.Value, position, lineNumber));
                remainingText = match.RemainingText.Trim();

                _logger.LogTrace($"|| {nameof(Tokenize)} | Match with [{match.TokenType}] was successful with value '{match.Value}'");
            }
            else
            {
                _logger.LogTrace($"|| {nameof(Tokenize)} | Remaining text could not be matched with any of the defined regular expressions");

                if (IsWhitespace(remainingText))
                {
                    remainingText = remainingText[1..];
                }
                else
                {
                    var invalidTokenMatch = CreateInvalidTokenMatch(remainingText, lineNumber, position);
                    tokens.Add(new DslToken(invalidTokenMatch.TokenType, invalidTokenMatch.Value, position, lineNumber));
                    remainingText = invalidTokenMatch.RemainingText;
                }
            }

            lineNumber   = startNewLine ? lineNumber + 1 : lineNumber;
            position     = startNewLine ? 1 : position + match.Value.Length;
            startNewLine = !startNewLine && lineBreaksInRemainingText.Count > 0;

            _logger.LogTrace($"|| {nameof(Tokenize)} | Remaining text: '{remainingText}'");
        }

        tokens.Add(new DslToken(TokenType.SequenceTerminator, string.Empty, position, lineNumber));

        _logger.LogTrace($"|| {nameof(Tokenize)} | Tokenizing process completed. Generated [{tokens.Count}] tokens");

        return tokens;
    }

    private TokenMatch FindMatch(string source)
    {
        foreach (var tokenDefinition in TokenDefinitions)
        {
            var match = tokenDefinition.Match(source);
            if (match.IsMatch) return match;
        }

        return new TokenMatch {IsMatch = false};
    }

    private static bool IsWhitespace(string source)
    {
        return Regex.IsMatch(source, "^\\s+");
    }

    private static TokenMatch CreateInvalidTokenMatch(string lqlText, int lineNumber, int position)
    {
        var match = Regex.Match(lqlText, "(^\\S+\\s)|^\\S+");
        if (match.Success)
        {
            return new TokenMatch
            {
                IsMatch       = true,
                RemainingText = lqlText[match.Length..],
                TokenType     = TokenType.Invalid,
                Value         = match.Value.Trim()
            };
        }

        throw new TokenizerException("Tokenizer failed to generate an invalid token from the remaining text. Please evaluate the regular expressions to" +
            "ensure correctness", remainingText: lqlText, lineNumber, position);
    }
}