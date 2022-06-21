namespace AspectSol.Lib.Infra.Exceptions;

public abstract class AspectSolException : Exception
{

    protected AspectSolException(string message) : base(message)
    {
    }
}