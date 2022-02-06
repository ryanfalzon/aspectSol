using System.Collections.Generic;
using System.Text;

namespace AspectSol.Lib.Domain.AST;

public class AspectNode : SelectorNode
{
    public string Name { get; set; }

    public List<StatementNode> Statements { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<DefinitionSelectorNode>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<Name>{Name}</Name>");
        stringBuilder.AppendLine(string.Join('\n', Statements));

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</DefinitionSelectorNode>");

        return stringBuilder.ToString();
    }
}