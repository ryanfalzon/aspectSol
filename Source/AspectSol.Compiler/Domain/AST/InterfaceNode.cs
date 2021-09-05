using AspectSol.Compiler.Infra.Enums;

namespace AspectSol.Compiler.Domain.AST
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