using System;

namespace AspectSol.Compiler.Infra.Exceptions
{
    public class InvalidVariableTypeSelectorNodeException : Exception
    {
        public InvalidVariableTypeSelectorNodeException(string message) : base(message)
        {
        }
    }
}