using AspectSol.Lib.Infra.Enums;
using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class VariableAccessNode : Node
{
    public VariableAccess Value { get; }
    
    public VariableAccessNode(VariableAccess value)
    {
        Value = value;
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(VariableAccessNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<Value>{Value}</Value>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(VariableAccessNode)}>");

        return stringBuilder.ToString();
    }
}