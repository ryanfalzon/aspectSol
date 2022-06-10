using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering;

public interface ISourceManipulation
{
    void AddContract(ref JToken source, JToken contract);
    void AddContracts(ref JToken source, List<JToken> contract);
    void AddInterfaceToContract(ref JToken source, JToken contractInterface, SelectionResult selectionResult);
    void AddDefinitionToContract(ref JToken source, JToken definition, SelectionResult selectionResult);
    void AddDefinitionsToContract(ref JToken source, List<JToken> definitions, SelectionResult selectionResult);
    void AddDefinitionToFunction(ref JToken source, JToken definition, SelectionResult selectionResult);
    void AddDefinitionsToFunction(ref JToken source, List<JToken> definitions, SelectionResult selectionResult);
    void UpdateContractName(ref JToken source, SelectionResult selectionResult, string name);
    void UpdateFunctionName(ref JToken source, SelectionResult selectionResult, string name);
    void UpdateFunctionVisibility(ref JToken source, SelectionResult selectionResult, string name);
}