using AspectSol.Lib.Domain.Filtering.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering;

public interface IFunctionFiltering
{
    FilteringResult FilterFunctionsByFunctionName(JToken jToken, string functionName);
    FilteringResult FilterFunctionsByVisibility(JToken jToken, string visibility);
    FilteringResult FilterFunctionsByStateMutability(JToken jToken, string stateMutability);
    FilteringResult FilterFunctionsByAllModifiers(JToken jToken, List<string> modifiers);
    FilteringResult FilterFunctionsByEitherModifiers(JToken jToken, List<string> modifiers);
    FilteringResult FilterFunctionsByModifier(JToken jToken, string modifier, bool invert);
    FilteringResult FilterFunctionsByParameters(JToken jToken, string parameterType, string parameterName);
    FilteringResult FilterFunctionsByParameters(JToken jToken, List<(string Type, string Value)> parameters);
    FilteringResult FilterFunctionsByReturnParameters(JToken jToken, string returnParameter);
    FilteringResult FilterFunctionsByReturnParameters(JToken jToken, List<string> returnParameters);
    FilteringResult FilterFunctionCallsByInstanceName(JToken jToken, string instanceName, string functionName);
    FilteringResult FilterFunctionsImplementedFromInterface(JToken jToken, string interfaceName, bool invert);
    FilteringResult FilterFunctionCalls(JToken jToken, string functionName);
}