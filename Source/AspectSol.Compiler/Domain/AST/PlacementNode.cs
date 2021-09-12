using AspectSol.Compiler.Infra.Enums;
using System.Text;

namespace AspectSol.Compiler.Domain.AST
{
    public class PlacementNode : Node
    {
        public Placement Value { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{GetIndentation()}<PlacementNode>");
            IncreaseIndentation();

            stringBuilder.AppendLine($"{GetIndentation()}<Value>{Value}</Value>");

            DecreaseIndentation();
            stringBuilder.AppendLine($"{GetIndentation()}</PlacementNode>");

            return stringBuilder.ToString();
        }
    }
}