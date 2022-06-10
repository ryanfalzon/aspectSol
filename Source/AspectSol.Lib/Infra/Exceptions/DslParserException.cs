namespace AspectSol.Lib.Infra.Exceptions;

public class DslParserException : Exception
{
    public DslParserException(ExceptionCode code, string message) : base(message)
    {
    }
}