using System;

namespace AspectSol.Compiler.Infra.Exceptions
{
    public class InvalidVariableNameSelectorNodeException : Exception
    {
        public InvalidVariableNameSelectorNodeException(string message) : base(message)
        {
        }
    }
}