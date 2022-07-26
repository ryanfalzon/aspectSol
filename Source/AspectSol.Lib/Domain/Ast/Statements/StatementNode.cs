using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.JavascriptExecution;
using AspectSol.Lib.Infra;
using AspectSol.Lib.Infra.Helpers;
using AspectSol.Lib.Infra.TemporaryStorage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Statements;

public abstract class StatementNode : ExecutableNode
{
    public IEnumerable<ExpressionNode> Body { get; set; }

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

    protected JToken EncodeBodyContent(SelectionResult selectionResult, TempStorageRepository tempStorageRepository, IJavascriptExecutor javascriptExecutor,
        SolidityAstNodeIdResolver solidityAstNodeIdResolver)
    {
        var bodyText = string.Join('\n', Body.Select(x => x.GetValue()));
        if (selectionResult.InterestedFunctions is {Count: > 0})
        {
            bodyText = SolidityPlaceholderFactory.GetFunctionStatementPlaceholder(bodyText);
        }
        else if (selectionResult.InterestedContracts is {Count: > 0})
        {
            bodyText = SolidityPlaceholderFactory.GetContractStatementPlaceholder(bodyText);
        }
        
        tempStorageRepository.Add(bodyText, out var filepath);

        var response = javascriptExecutor.GenerateAst($"{filepath}").GetAwaiter().GetResult();
        var deserializedResponse = JsonConvert.DeserializeObject<JavascriptResponse>(response);

        var parsedContract = JToken.Parse(deserializedResponse?.Data ?? string.Empty);
        var contractAst = parsedContract["sources"]?.First?.First?["ast"];

        return contractAst;
    }
}