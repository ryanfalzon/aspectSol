using System.Collections.Generic;
using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class AppendStatementNode : StatementNode
{
    public PlacementNode Placement { get; set; }

    public LocationNode Location { get; set; }

    public List<SelectorNode> Selectors { get; set; }

    public SenderNode Sender { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<AppendStatementNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine(Placement.ToString());
        stringBuilder.AppendLine(Location.ToString());
        stringBuilder.AppendLine(string.Join('\n', Selectors));
        stringBuilder.AppendLine(Sender.ToString());
        stringBuilder.AppendLine(base.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</AppendStatementNode>");

        return stringBuilder.ToString();
    }
}