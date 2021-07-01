using AspectSol.Compiler.Models.Enums;

namespace AspectSol.Compiler.Models.AST
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