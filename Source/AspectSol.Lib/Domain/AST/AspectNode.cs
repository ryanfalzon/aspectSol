using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.JavascriptExecution;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.AST;

public class AspectNode : ExecutableNode
{
    public string Name { get; init; }

    public List<StatementNode> Statements { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(AspectNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine($"{GetIndentation()}<Name>{Name}</Name>");
        stringBuilder.AppendLine(string.Join('\n', Statements));

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(AspectNode)}>");

        return stringBuilder.ToString();
    }

    public override JToken Execute(JToken smartContract, AbstractFilteringService abstractFilteringService, IJavascriptExecutor javascriptExecutor)
    {
        var updatedSmartContract = smartContract;

        foreach (var statement in Statements)
        {
            updatedSmartContract = statement.Execute(updatedSmartContract, abstractFilteringService, javascriptExecutor);
        }

        return updatedSmartContract;
    }
}