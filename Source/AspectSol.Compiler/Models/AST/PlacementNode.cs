using AspectSol.Compiler.Models.Enums;

namespace AspectSol.Compiler.Models.AST
{
    public class PlacementNode : Node
    {
        public Placement Placement { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}