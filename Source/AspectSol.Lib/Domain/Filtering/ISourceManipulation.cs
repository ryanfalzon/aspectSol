using AspectSol.Lib.Domain.Filtering.FilteringResults;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering;

public interface ISourceManipulation
{
    void AddContract(ref JToken source, JToken contract);
    void AddContracts(ref JToken source, List<JToken> contract);
    void AddInterfaceToContract(ref JToken source, JToken contractInterface, FilteringResult selectionResult);
    void AddDefinitionToContract(ref JToken source, JToken definition, FilteringResult selectionResult, bool addFirst = true);
    void AddDefinitionsToContract(ref JToken source, List<JToken> definitions, FilteringResult filteringResult, bool addFirst = true);
    void AddDefinitionToFunction(ref JToken source, JToken definition, FilteringResult filteringResult, bool addFirst = true);
    void AddDefinitionsToFunction(ref JToken source, List<JToken> definitions, FilteringResult filteringResult, bool addFirst = true);
    void UpdateContractName(ref JToken source, FilteringResult filteringResult, string name);
    void UpdateFunctionName(ref JToken source, FilteringResult filteringResult, string name);
    void UpdateFunctionVisibility(ref JToken source, FilteringResult filteringResult, string name);
}