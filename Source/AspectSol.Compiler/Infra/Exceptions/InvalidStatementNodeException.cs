using System;

namespace AspectSol.Compiler.Infra.Exceptions
{
    public class InvalidStatementNodeException : Exception
    {
        public InvalidStatementNodeException(string message) : base(message)
        {
        }
    }
}