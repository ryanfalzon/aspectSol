using AspectSol.Compiler.Infra.Enums;

namespace AspectSol.Compiler.Domain.AST
{
    public class VariableDecoratorNode : DecoratorNode
    {
        public VariableVisibility VariableVisibility { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}