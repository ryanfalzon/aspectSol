using System.Collections.Generic;
using System.Text;

namespace AspectSol.Compiler.Domain.AST
{
    public class ReturnSelectorNode : SelectorNode
    {
        public List<ReturnTypeNode> ReturnTypes { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{GetIndentation()}<ReturnSelectorNode>");
            IncreaseIndentation();

            stringBuilder.AppendLine(string.Join('n', ReturnTypes));

            DecreaseIndentation();
            stringBuilder.AppendLine($"{GetIndentation()}</ReturnSelectorNode>");

            return stringBuilder.ToString();
        }
    }
}