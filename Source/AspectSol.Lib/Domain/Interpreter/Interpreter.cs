using AspectSol.Lib.Domain.Ast;
using AspectSol.Lib.Domain.Filtering;
using AspectSol.Lib.Domain.JavascriptExecution;
using AspectSol.Lib.Infra.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Interpreter;

public class Interpreter : IInterpreter
{
    private readonly IJavascriptExecutor _javascriptExecutor;
    private readonly TempStorageRepository _tempStorageRepository;
    private readonly AbstractFilteringService _abstractFilteringService;
    private readonly SolidityAstNodeIdResolver _solidityAstNodeIdResolver;

    public Interpreter(IJavascriptExecutor javascriptExecutor, TempStorageRepository tempStorageRepository, AbstractFilteringService abstractFilteringService,
        SolidityAstNodeIdResolver solidityAstNodeIdResolver)
    {
        _javascriptExecutor        = javascriptExecutor;
        _tempStorageRepository     = tempStorageRepository;
        _abstractFilteringService  = abstractFilteringService;
        _solidityAstNodeIdResolver = solidityAstNodeIdResolver;
    }

    public async Task<string> Interpret(AspectNode aspectNode, string smartContract)
    {
        _tempStorageRepository.Add(smartContract, out var filepath);

        var response = await _javascriptExecutor.GenerateAst($"{filepath}");
        var deserializedResponse = JsonConvert.DeserializeObject<JavascriptResponse>(response ?? string.Empty);

        var parsedContract = JToken.Parse(deserializedResponse?.Data ?? string.Empty);
        var contractAst = parsedContract["sources"]?.First?.First?["ast"];

        aspectNode.Execute(contractAst, _abstractFilteringService, _javascriptExecutor, _tempStorageRepository, _solidityAstNodeIdResolver);
        var code = await _javascriptExecutor.Execute("generateCode", new object[] {_solidityAstNodeIdResolver.UpdateNodeIdentifiers(JsonConvert.SerializeObject(parsedContract))});

        return code;
    }
}