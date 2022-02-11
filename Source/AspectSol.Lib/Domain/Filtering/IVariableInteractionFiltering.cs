using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering;

public interface IVariableInteractionFiltering
{
    SelectionResult FilterVariableInteractionByVariableName(JToken jToken, string variableName);
    SelectionResult FilterVariableInteractionByVariableType(JToken jToken, string variableType);
    SelectionResult FilterVariableInteractionByVariableVisibility(JToken jToken, string variableVisibility);
    SelectionResult FilterVariableInteractionByVariableAccessKey(JToken jToken, string accessKey);
}