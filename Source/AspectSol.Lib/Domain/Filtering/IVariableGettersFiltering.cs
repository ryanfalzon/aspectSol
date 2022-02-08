using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering;

public interface IVariableGettersFiltering
{
    SelectionResult FilterVariableGettersByVariableName(JToken jToken, string variableName);
    SelectionResult FilterVariableGettersByVariableType(JToken jToken, string variableType);
    SelectionResult FilterVariableGettersByVariableVisibility(JToken jToken, string variableVisibility);
    SelectionResult FilterVariableGettersByVariableAccessKey(JToken jToken, string accessKey);
}