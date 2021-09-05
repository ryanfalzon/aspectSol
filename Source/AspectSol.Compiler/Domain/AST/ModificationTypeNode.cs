using AspectSol.Compiler.Infra.Enums;

namespace AspectSol.Compiler.Domain.AST
{
    public class ModificationTypeNode : Node
    {
        public ModificationType ModificationType { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}