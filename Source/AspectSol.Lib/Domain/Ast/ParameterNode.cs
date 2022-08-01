using System.Text;

namespace AspectSol.Lib.Domain.Ast;

public class ParameterNode : Node
{
    public string Type { get; init; }

    public string Name { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(ParameterNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<Type>{Type}</Type>");
        stringBuilder.AppendLine($"{GetIndentation()}<Name>{Name}</Name>");

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(ParameterNode)}>");

        return stringBuilder.ToString();
    }
}