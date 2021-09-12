using System;

namespace AspectSol.Compiler.Infra.Exceptions
{
    public class InvalidVariableAccessNodeException : Exception
    {
        public InvalidVariableAccessNodeException(string message) : base(message)
        {
        }
    }
}