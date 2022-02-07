using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering;

public interface IVariableDefinitionFiltering
{
    SelectionResult FilterVariableDefinitionByVariableType(JToken jToken, string variableType);
    SelectionResult FilterVariableDefinitionByVariableName(JToken jToken, string variableName);
}