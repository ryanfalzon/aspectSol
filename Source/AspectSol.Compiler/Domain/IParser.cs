using AspectSol.Compiler.Models.AST;
using System.Collections.Generic;

namespace AspectSol.Compiler.Domain
{
    public interface IParser
    {
        List<StatementNode> Start(List<string> tokens);
    }
}