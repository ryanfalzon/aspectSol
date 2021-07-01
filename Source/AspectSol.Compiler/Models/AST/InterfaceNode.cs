using AspectSol.Compiler.Models.Enums;

namespace AspectSol.Compiler.Models.AST
{
    public class InterfaceNode : Node
    {
        public Interface Interface { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}