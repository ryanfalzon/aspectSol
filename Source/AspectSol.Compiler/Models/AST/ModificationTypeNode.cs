using AspectSol.Compiler.Models.Enums;

namespace AspectSol.Compiler.Models.AST
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