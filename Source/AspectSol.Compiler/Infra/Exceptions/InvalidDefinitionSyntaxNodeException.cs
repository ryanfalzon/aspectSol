using System;

namespace AspectSol.Compiler.Infra.Exceptions
{
    public class InvalidDefinitionSyntaxNodeException : Exception
    {
        public InvalidDefinitionSyntaxNodeException(string message) : base(message)
        {
        }
    }
}