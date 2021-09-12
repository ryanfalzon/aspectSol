using System;

namespace AspectSol.Compiler.Infra.Exceptions
{
    public class InvalidFunctionSelectorNodeException : Exception
    {
        public InvalidFunctionSelectorNodeException(string message) : base(message)
        {
        }
    }
}