using System.Text;

namespace AspectSol.Compiler.Domain.AST
{
    public class NameInstanceSelectorNode : InstanceSelectorNode
    {
        public string InstanceName { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{GetIndentation()}<NameInstanceSelectorNode>");
            IncreaseIndentation();

            stringBuilder.AppendLine($"{GetIndentation()}<InstanceName>{InstanceName}</InstanceName>");

            DecreaseIndentation();
            stringBuilder.AppendLine($"{GetIndentation()}</NameInstanceSelectorNode>");

            return stringBuilder.ToString();
        }
    }
}