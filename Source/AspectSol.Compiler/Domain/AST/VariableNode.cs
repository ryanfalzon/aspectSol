using AspectSol.Compiler.Infra.Enums;

namespace AspectSol.Compiler.Domain.AST
{
    public class VariableNode : Node
    {
        public Variable Variable { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}