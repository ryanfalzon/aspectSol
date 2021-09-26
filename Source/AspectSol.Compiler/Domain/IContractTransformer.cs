using AspectSol.Compiler.Infra.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AspectSol.Compiler.Domain
{
    public interface IContractTransformer
    {
        SelectionResult FilterContractsByContractName(JToken jToken, string contractName);
        SelectionResult FilterContractsByInterfaceName(JToken container, string interfaceName);

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

        SelectionResult FilterVariableDefinitionByVariableType(JToken jToken, string variableType);
        SelectionResult FilterVariableDefinitionByVariableName(JToken jToken, string variableName);
    }
}