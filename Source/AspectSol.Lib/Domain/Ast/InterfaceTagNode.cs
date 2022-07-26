using System.Text;
using AspectSol.Lib.Infra.Enums;

namespace AspectSol.Lib.Domain.Ast;

public class InterfaceTagNode : Node
{
    public InterfaceTag Value { get; }
    
    public InterfaceTagNode(InterfaceTag value)
    {
        Value = value;
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(InterfaceTagNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<Value>{Value}</Value>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(InterfaceTagNode)}>");

        return stringBuilder.ToString();
    }
}