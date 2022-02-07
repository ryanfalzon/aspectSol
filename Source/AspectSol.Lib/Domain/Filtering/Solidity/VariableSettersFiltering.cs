using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering.Solidity;

public class VariableSettersFiltering : IVariableSettersFiltering
{
    public SelectionResult FilterVariableSettersByVariableName(JToken jToken, string variableName)
    {
        throw new NotImplementedException();
    }

    public SelectionResult FilterVariableSettersByVariableType(JToken jToken, string variableType)
    {
        throw new NotImplementedException();
    }

    public SelectionResult FilterVariableSettersByVisibility(JToken jToken, string variableVisibility)
    {
        throw new NotImplementedException();
    }

    public SelectionResult FilterVariableSettersByAccessKey(JToken jToken, string accessKey)
    {
        throw new NotImplementedException();
    }
}