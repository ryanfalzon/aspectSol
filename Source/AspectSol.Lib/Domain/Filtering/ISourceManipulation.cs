using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering;

public interface ISourceManipulation
{
    void AddContract(ref JToken source, JToken contract);
    void AddContracts(ref JToken source, List<JToken> contract);
    void AddInterfaceToContract(ref JToken source, JToken contractInterface, SelectionResult selectionResult);
    void AddDefinitionToContract(ref JToken source, JToken definition, SelectionResult selectionResult);
    void AddDefinitionToContract(ref JToken source, List<JToken> definitions, SelectionResult selectionResult);
    void AddDefinitionToFunction(ref JToken source, JToken definition, SelectionResult selectionResult);
    void AddDefinitionToFunction(ref JToken source, List<JToken> definitions, SelectionResult selectionResult);
    void AddTagToFunction(ref JToken source, string tag, SelectionResult selectionResult);
    void AddTagsToFunction(ref JToken source, List<string> tags, SelectionResult selectionResult);
    void UpdateContractName(ref JToken source, SelectionResult selectionResult, string name);
    void UpdateFunctionName(ref JToken source, SelectionResult selectionResult, string name);
}