using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering;

public interface ISourceManipulation
{
    void AddContract(ref JToken source, JToken contract);
    void AddContracts(ref JToken source, List<JToken> contract);
    void AddInterfaceToContract(ref JToken source, JToken contractInterface, SelectionResult selectionResult);
    void AddDefinitionToContract(ref JToken contract, JToken definition, SelectionResult selectionResult, bool addFirst = true);
    void AddDefinitionsToContract(ref JToken contract, List<JToken> definitions, SelectionResult selectionResult, bool addFirst = true);
    void AddDefinitionToFunction(ref JToken contract, JToken definition, SelectionResult selectionResult, bool addFirst = true);
    void AddDefinitionsToFunction(ref JToken source, List<JToken> definitions, SelectionResult selectionResult, bool addFirst = true);
    void UpdateContractName(ref JToken source, SelectionResult selectionResult, string name);
    void UpdateFunctionName(ref JToken source, SelectionResult selectionResult, string name);
    void UpdateFunctionVisibility(ref JToken source, SelectionResult selectionResult, string name);
}