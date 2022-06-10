using AspectSol.Lib.Infra.Enums;
using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class PlacementNode : Node
{
    public Placement Value { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(PlacementNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<Value>{Value}</Value>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(PlacementNode)}>");

        return stringBuilder.ToString();
    }
}