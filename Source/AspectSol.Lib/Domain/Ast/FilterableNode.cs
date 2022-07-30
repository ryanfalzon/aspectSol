using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.Filtering.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public abstract class FilterableNode : Node
{
    /// <summary> 
    /// Filter the provided smart contract using the properties encoded within this AST node
    /// </summary>
    /// <param name="smartContract"></param>
    /// <param name="abstractFilteringService"></param>
    /// <returns>Selection result which contains the updated JSON encoded smart contract</returns>
    public abstract FilteringResult Filter(JToken smartContract, AbstractFilteringService abstractFilteringService);
}