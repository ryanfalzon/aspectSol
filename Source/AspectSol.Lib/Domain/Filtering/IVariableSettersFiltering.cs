using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering;

public interface IVariableSettersFiltering
{
    SelectionResult FilterVariableSettersByVariableName(JToken jToken, string variableName);
    SelectionResult FilterVariableSettersByVariableType(JToken jToken, string variableType);
    SelectionResult FilterVariableSettersByVisibility(JToken jToken, string variableVisibility);
    SelectionResult FilterVariableSettersByAccessKey(JToken jToken, string accessKey);
}