using AspectSol.Lib.Domain.Filtering.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering;

public interface IVariableInteractionFiltering
{
    FilteringResult FilterVariableInteractionByVariableName(JToken jToken, string variableName);
    FilteringResult FilterVariableInteractionByVariableType(JToken jToken, string variableType);
    FilteringResult FilterVariableInteractionByVariableVisibility(JToken jToken, string variableVisibility);
    FilteringResult FilterVariableInteractionByVariableAccessKey(JToken jToken, string accessKey);
}