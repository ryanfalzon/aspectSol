using System;

namespace AspectSol.Compiler.Infra.Exceptions
{
    public class InvalidMappingException : Exception
    {
        public InvalidMappingException(string message) : base(message)
        {
        }
    }
}