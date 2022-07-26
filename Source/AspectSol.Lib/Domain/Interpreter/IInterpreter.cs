using AspectSol.Lib.Domain.Ast;

namespace AspectSol.Lib.Domain.Interpreter;

public interface IInterpreter
{
    public Task<string> Interpret(AspectNode aspectNode, string smartContract);
}