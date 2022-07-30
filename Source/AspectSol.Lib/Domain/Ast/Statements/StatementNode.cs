using System.Text;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Domain.JavascriptExecution;
using AspectSol.Lib.Infra.Helpers;
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

    protected JToken EncodeBodyContent(FilteringResult filteringResult, TempStorageRepository tempStorageRepository, IJavascriptExecutor javascriptExecutor)
    {
        var bodyText = string.Join('\n', Body.Select(x => x.GetValue()));
        if (filteringResult.ContractFilteringResults.Any(x => x.FunctionFilteringResults.Count > 0))
        {
            bodyText = SolidityPlaceholderFactory.GetFunctionStatementPlaceholder(bodyText);
        }
        else if (filteringResult.ContractFilteringResults.Any())
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