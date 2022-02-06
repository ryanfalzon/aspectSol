using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class NameVariableNameSelectorNode : VariableNameSelectorNode
{
    public string VariableName { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<NameVariableNameSelectorNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<VariableName>{VariableName}</VariableName>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</NameVariableNameSelectorNode>");

        return stringBuilder.ToString();
    }
}