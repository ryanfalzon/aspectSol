using System;

namespace AspectSol.Compiler.Infra.Exceptions
{
    public class InvalidPlacementNodeException : Exception
    {
        public InvalidPlacementNodeException(string message) : base(message)
        {
        }
    }
}