using System;

namespace AspectSol.Compiler.Infra.Exceptions
{
    public class InvalidSelectorNodeException : Exception
    {
        public InvalidSelectorNodeException(string message) : base(message)
        {
        }
    }
}