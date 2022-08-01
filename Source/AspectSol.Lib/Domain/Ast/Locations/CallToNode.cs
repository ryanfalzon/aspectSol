using System.Text;

namespace AspectSol.Lib.Domain.Ast.Locations;

public class CallToNode : LocationNode
{
    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(CallToNode)}>");
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(CallToNode)}>");

        return stringBuilder.ToString();
    }
}