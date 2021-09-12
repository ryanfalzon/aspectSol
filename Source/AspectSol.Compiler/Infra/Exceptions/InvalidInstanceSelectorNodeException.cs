using System;

namespace AspectSol.Compiler.Infra.Exceptions
{
    public class InvalidInstanceSelectorNodeException : Exception
    {
        public InvalidInstanceSelectorNodeException(string message) : base(message)
        {
        }
    }
}