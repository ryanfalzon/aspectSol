using AspectSol.Lib.Domain.Filtering.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering;

public interface IVariableDefinitionFiltering
{
    FilteringResult FilterVariableDefinitionByVariableType(JToken jToken, string variableType);
    FilteringResult FilterVariableDefinitionByVariableName(JToken jToken, string variableName);
}