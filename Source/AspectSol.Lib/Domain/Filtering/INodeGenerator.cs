using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering;

public interface INodeGenerator
{
    JToken GenerateContractInterfaceNode(string interfaceName);
    JToken GenerateFunctionModifierNode(string modifierName);
}