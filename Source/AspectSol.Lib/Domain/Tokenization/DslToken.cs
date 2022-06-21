namespace AspectSol.Lib.Domain.Tokenization;

public class DslToken
{
    public TokenType TokenType { get; }
    public string Value { get; }
    public int Position { get; }
    public int LineNumber { get; }

    public DslToken(TokenType tokenType, int position, int lineNumber)
    {
        TokenType  = tokenType;
        Position   = position;
        LineNumber = lineNumber;
        Value      = string.Empty;
    }

    public DslToken(TokenType tokenType, string value, int position, int lineNumber)
    {
        TokenType  = tokenType;
        Value      = value;
        Position   = position;
        LineNumber = lineNumber;
    }

    public DslToken Clone()
    {
        return new DslToken(TokenType, Value, Position, LineNumber);
    }
}