using AspectSol.Compiler.Infra.Enums;
using System.Text;

namespace AspectSol.Compiler.Domain.AST
{
    public class InterfaceNode : Node
    {
        public Interface Value { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{GetIndentation()}<InterfaceNode>");
            IncreaseIndentation();

            stringBuilder.AppendLine($"{GetIndentation()}<Value>{Value}</Value>");

            DecreaseIndentation();
            stringBuilder.AppendLine($"{GetIndentation()}</InterfaceNode>");

            return stringBuilder.ToString();
        }
    }
}