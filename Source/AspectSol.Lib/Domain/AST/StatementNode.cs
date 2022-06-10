using System.Text;

namespace AspectSol.Lib.Domain.AST;

public abstract class StatementNode : ExecutableNode
{
    protected IEnumerable<ExpressionNode> Body { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<Body>");
        IncreaseIndentation();

        stringBuilder.AppendLine(string.Join('n', Body));

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</Body>");

        return stringBuilder.ToString();
    }
}