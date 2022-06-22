namespace AspectSol.Lib.Infra.Exceptions;

public class ParserException : AspectSolException
{
    public string ShortMessage { get; }
    public int LineNumber { get; }
    public int Position { get; }

    public ParserException(string detailedMessage, int lineNumber, int position) : base(detailedMessage)
    {
        LineNumber = lineNumber;
        Position   = position;
    }

    public ParserException(string detailedMessage, string shortMessage, int lineNumber, int position) : base(detailedMessage)
    {
        ShortMessage = shortMessage;
        LineNumber   = lineNumber;
        Position     = position;
    }
}