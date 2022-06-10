using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.JavascriptExecution;
using AspectSol.Lib.Infra;
using AspectSol.Lib.Infra.TemporaryStorage;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.AST;

public class StatementAppendNode : StatementNode
{
    public PlacementNode Placement { get; set; }

    public LocationNode Location { get; set; }

    public List<SelectorNode> Selectors { get; set; }

    public SenderNode Sender { get; set; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(StatementAppendNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(Placement.ToString());
        stringBuilder.AppendLine(Location.ToString());
        stringBuilder.AppendLine(string.Join('\n', Selectors));
        stringBuilder.AppendLine(Sender.ToString());
        stringBuilder.AppendLine(base.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(StatementAppendNode)}>");

        return stringBuilder.ToString();
    }

    public override JToken Execute(JToken smartContract, AbstractFilteringService abstractFilteringService, IJavascriptExecutor javascriptExecutor)
    {
        // TODO - Execute this node

        var statements = Body.Select(item => item as ExpressionGenericNode).Select(item => item?.Expression);
        var tempContent = SolidityPlaceholderFactory.GetStatementsPlaceholder(statements);
        
        TempStorageRepository.Add(tempContent, out var filename);
        var bodyAst = javascriptExecutor.Execute("generateAst", new object[] { $"tmp/{filename}.txt" }).Result;
    }
}