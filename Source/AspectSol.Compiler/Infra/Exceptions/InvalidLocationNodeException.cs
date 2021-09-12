using System;

namespace AspectSol.Compiler.Infra.Exceptions
{
    public class InvalidLocationNodeException : Exception
    {
        public InvalidLocationNodeException(string message) : base(message)
        {
        }
    }
}