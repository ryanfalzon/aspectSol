using AspectSol.Compiler.Models.AST;
using System.Collections.Generic;

namespace AspectSol.Compiler.Domain
{
    public interface IExecutor
    {
        string Start(List<StatementNode> statements);
    }
}