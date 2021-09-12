using System.Text;

namespace AspectSol.Compiler.Domain.AST
{
    public class ModifierNode : Node
    {
        public string ModifierName { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{GetIndentation()}<ModifierNode>");
            IncreaseIndentation();

            stringBuilder.AppendLine($"{GetIndentation()}<ModifierName>{ModifierName}</ModifierName>");

            DecreaseIndentation();
            stringBuilder.AppendLine($"{GetIndentation()}</ModifierNode>");

            return stringBuilder.ToString();
        }
    }
}