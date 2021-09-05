using AspectSol.Compiler.Domain.AST;
using System.Collections.Generic;

namespace AspectSol.Compiler.Domain
{
    public interface IParser
    {
        List<StatementNode> Start(List<string> tokens);
    }
}