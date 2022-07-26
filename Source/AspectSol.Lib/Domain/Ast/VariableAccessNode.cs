using System.Text;
using AspectSol.Lib.Infra.Enums;

namespace AspectSol.Lib.Domain.Ast;

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