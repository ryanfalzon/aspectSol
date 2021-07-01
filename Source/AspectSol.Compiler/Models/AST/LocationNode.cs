using AspectSol.Compiler.Models.Enums;

namespace AspectSol.Compiler.Models.AST
{
    public class LocationNode : Node
    {
        public Location Location { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}