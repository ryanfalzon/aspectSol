using System;

namespace AspectSol.Compiler.Infra.Exceptions
{
    public class InvalidAspectExpressionNodeException : Exception
    {
        public InvalidAspectExpressionNodeException(string message) : base(message)
        {
        }
    }
}