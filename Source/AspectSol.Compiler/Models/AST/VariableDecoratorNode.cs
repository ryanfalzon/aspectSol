using AspectSol.Compiler.Models.Enums;

namespace AspectSol.Compiler.Models.AST
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