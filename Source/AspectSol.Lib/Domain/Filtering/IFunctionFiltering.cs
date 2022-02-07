using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering;

public interface IFunctionFiltering
{
    SelectionResult FilterFunctionsByFunctionName(JToken jToken, string functionName);
    SelectionResult FilterFunctionsByVisibility(JToken jToken, string visibility);
    SelectionResult FilterFunctionsByStateMutability(JToken jToken, string stateMutability);
    SelectionResult FilterFunctionsByAllModifiers(JToken jToken, List<string> modifiers);
    SelectionResult FilterFunctionsByEitherModifiers(JToken jToken, List<string> modifiers);
    SelectionResult FilterFunctionsByModifier(JToken jToken, string modifier, bool invert);
    SelectionResult FilterFunctionsByParameters(JToken jToken, string parameterType, string parameterName);
    SelectionResult FilterFunctionsByParameters(JToken jToken, List<(string Type, string Value)> parameters);
    SelectionResult FilterFunctionsByReturnParameters(JToken jToken, string returnParameter);
    SelectionResult FilterFunctionsByReturnParameters(JToken jToken, List<string> returnParameters);
    SelectionResult FilterFunctionCallsByInstanceName(JToken jToken, string instanceName, string functionName);
    SelectionResult FilterFunctionsImplementedFromInterface(JToken jToken, string interfaceName, bool invert);
}