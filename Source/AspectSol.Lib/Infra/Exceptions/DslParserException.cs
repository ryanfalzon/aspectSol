using System;

namespace AspectSol.Lib.Infra.Exceptions;

public class DslParserException : Exception
{
    public DslParserException(string message) : base(message)
    {
    }
}