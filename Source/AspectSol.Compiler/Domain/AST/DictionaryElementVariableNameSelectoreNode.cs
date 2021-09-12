using System.Text;

namespace AspectSol.Compiler.Domain.AST
{
    public class DictionaryElementVariableNameSelectoreNode : VariableNameSelectorNode
    {
        public string VariableName { get; set; }

        public SelectorNode KeySelector { get; set; }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"{GetIndentation()}<DictionaryElementVariableNameSelectoreNode>");
            IncreaseIndentation();

            stringBuilder.AppendLine($"{GetIndentation()}<VariableName>{VariableName}</VariableName>");
            stringBuilder.AppendLine(KeySelector.ToString());

            DecreaseIndentation();
            stringBuilder.AppendLine($"{GetIndentation()}</DictionaryElementVariableNameSelectoreNode>");

            return stringBuilder.ToString();
        }
    }
}