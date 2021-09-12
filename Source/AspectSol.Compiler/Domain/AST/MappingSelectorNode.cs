using System.Text;

namespace AspectSol.Compiler.Domain.AST
{
    public class MappingSelectorNode : SelectorNode
    {
        public string MappingName { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{GetIndentation()}<MappingSelectorNode>");
            IncreaseIndentation();

            stringBuilder.AppendLine($"{GetIndentation()}<MappingName>{MappingName}</MappingName>");

            DecreaseIndentation();
            stringBuilder.AppendLine($"{GetIndentation()}</MappingSelectorNode>");

            return stringBuilder.ToString();
        }
    }
}