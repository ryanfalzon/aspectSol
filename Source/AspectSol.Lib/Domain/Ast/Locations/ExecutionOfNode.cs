using System.Text;

namespace AspectSol.Lib.Domain.Ast.Locations;

public class ExecutionOfNode : LocationNode
{
    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(ExecutionOfNode)}>");
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(ExecutionOfNode)}>");

        return stringBuilder.ToString();
    }
}