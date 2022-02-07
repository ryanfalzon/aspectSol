using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering.Solidity;

public class SourceManipulation : ISourceManipulation
{
    public void AddContract(ref JToken source, JToken contract)
    {
        throw new NotImplementedException();
    }

    public void AddContracts(ref JToken source, List<JToken> contract)
    {
        throw new NotImplementedException();
    }

    public void AddInterfaceToContract(ref JToken source, JToken contractInterface, SelectionResult selectionResult)
    {
        throw new NotImplementedException();
    }

    public void AddDefinitionToContract(ref JToken source, JToken definition, SelectionResult selectionResult)
    {
        throw new NotImplementedException();
    }

    public void AddDefinitionToContract(ref JToken source, List<JToken> definitions, SelectionResult selectionResult)
    {
        throw new NotImplementedException();
    }

    public void AddDefinitionToFunction(ref JToken source, JToken definition, SelectionResult selectionResult)
    {
        throw new NotImplementedException();
    }

    public void AddDefinitionToFunction(ref JToken source, List<JToken> definitions, SelectionResult selectionResult)
    {
        throw new NotImplementedException();
    }

    public void AddTagToFunction(ref JToken source, string tag, SelectionResult selectionResult)
    {
        throw new NotImplementedException();
    }

    public void AddTagsToFunction(ref JToken source, List<string> tags, SelectionResult selectionResult)
    {
        throw new NotImplementedException();
    }

    public void UpdateContractName(ref JToken source, SelectionResult selectionResult, string name)
    {
        throw new NotImplementedException();
    }

    public void UpdateFunctionName(ref JToken source, SelectionResult selectionResult, string name)
    {
        throw new NotImplementedException();
    }
}