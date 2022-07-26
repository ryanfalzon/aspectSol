using System.Text;
using AspectSol.Lib.Infra.Enums;

namespace AspectSol.Lib.Domain.Ast;

public class LocationNode : Node
{
    public Location Value { get; }
    
    public LocationNode(Location value)
    {
        Value = value;
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(LocationNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<Value>{Value}</Value>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(LocationNode)}>");

        return stringBuilder.ToString();
    }
}