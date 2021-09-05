using AspectSol.Compiler.Infra.Enums;

namespace AspectSol.Compiler.Domain.AST
{
    public class LocationNode : Node
    {
        public Location Value { get; set; }

        public override void Visit()
        {
            throw new System.NotImplementedException();
        }
    }
}