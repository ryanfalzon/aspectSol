using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.JavascriptExecution;
using AspectSol.Lib.Infra.Enums;
using AspectSol.Lib.Infra.Helpers;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Ast;

public abstract class ExecutableNode : Node
{
    /// <summary> 
    /// Executes the AST node on the passed JSON encoded smart contract AST 
    /// </summary>
    /// <param name="smartContract"></param>
    /// <param name="abstractFilteringService"></param>
    /// <param name="javascriptExecutor"></param>
    /// <param name="tempStorageRepository"></param>
    /// <returns>JToken containing the updated JSON encoded smart contract</returns>
    public abstract JToken Execute(JToken smartContract, AbstractFilteringService abstractFilteringService, IJavascriptExecutor javascriptExecutor,
        TempStorageRepository tempStorageRepository);
}