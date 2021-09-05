using AspectSol.Compiler.Infra.Enums;

namespace AspectSol.Compiler.Domain.AST
{
    public class PlacementNode : Node
    {
        public Placement Value { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}