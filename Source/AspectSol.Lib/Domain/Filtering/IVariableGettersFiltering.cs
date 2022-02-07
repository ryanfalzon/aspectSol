using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering;

public interface IVariableGettersFiltering
{
    SelectionResult FilterVariableGettersByVariableName(JToken jToken, string variableName);
    SelectionResult FilterVariableGettersByVariableType(JToken jToken, string variableType);
    SelectionResult FilterVariableGettersByVisibility(JToken jToken, string variableVisibility);
    SelectionResult FilterVariableGettersByAccessKey(JToken jToken, string accessKey);
}