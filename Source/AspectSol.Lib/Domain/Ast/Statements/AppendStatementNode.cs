using System.Text;
using AspectSol.Lib.Domain.Ast.Locations;
using AspectSol.Lib.Domain.Ast.Placements;
using AspectSol.Lib.Domain.Ast.Selectors;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.JavascriptExecution;
using AspectSol.Lib.Infra.Helpers;
using AspectSol.Lib.Infra.Helpers.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast.Statements;

public class AppendStatementNode : StatementNode
{
    public PlacementNode Placement { get; init; }
    public LocationNode Location { get; init; }
    public List<SelectorNode> Selectors { get; init; }
    public SenderNode Sender { get; init; }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();

        stringBuilder.AppendLine($"{GetIndentation()}<{nameof(AppendStatementNode)}>");
        IncreaseIndentation();

        stringBuilder.AppendLine(Placement.ToString());
        stringBuilder.AppendLine(Location.ToString());
        stringBuilder.AppendLine(string.Join('\n', Selectors));
        stringBuilder.AppendLine(Sender.ToString());
        stringBuilder.AppendLine(base.ToString());

        DecreaseIndentation();
        stringBuilder.AppendLine($"{GetIndentation()}</{nameof(AppendStatementNode)}>");

        return stringBuilder.ToString();
    }

    // TODO - Location and sender not yet supported in the execution of this method
    public override JToken Execute(JToken smartContract, AbstractFilteringService abstractFilteringService, IJavascriptExecutor javascriptExecutor,
        TempStorageRepository tempStorageRepository, SolidityAstNodeIdResolver solidityAstNodeIdResolver)
    {
        var filteringResults = Selectors.Select(x => x.Filter(smartContract, abstractFilteringService)).ToArray();
        var intersectionResult = FilteringResultHelpers.Intersect(filteringResults);
        var encodedBody = EncodeBodyContent(intersectionResult, tempStorageRepository, javascriptExecutor);
        
        return Placement.Evaluate(smartContract, intersectionResult, encodedBody, abstractFilteringService);
    }
}