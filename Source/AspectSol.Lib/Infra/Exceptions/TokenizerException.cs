namespace AspectSol.Lib.Infra.Exceptions;

public class TokenizerException : AspectSolException
{
    public string RemainingText { get; }
    public int LineNumber { get; }
    public int Position { get; }

    public TokenizerException(string message, string remainingText, int lineNumber, int position) : base(message)
    {
        RemainingText = remainingText;
        LineNumber    = lineNumber;
        Position      = position;
    }
}