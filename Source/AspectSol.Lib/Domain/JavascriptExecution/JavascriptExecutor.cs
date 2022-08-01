using Jering.Javascript.NodeJS;

namespace AspectSol.Lib.Domain.JavascriptExecution;

public class JavascriptExecutor : IJavascriptExecutor
{
    private readonly INodeJSService _nodeJsService;
    private readonly ScriptFactory _scriptFactory;

    public JavascriptExecutor(INodeJSService nodeJsService, ScriptFactory scriptFactory)
    {
        _nodeJsService = nodeJsService ?? throw new ArgumentNullException(nameof(nodeJsService));
        _scriptFactory = scriptFactory ?? throw new ArgumentNullException(nameof(scriptFactory));
    }

    public async Task<string> Execute(string scriptName, object[] arguments)
    {
        var script = await _scriptFactory.GetScript(scriptName);
        return await _nodeJsService.InvokeFromStringAsync<string>(script, args: arguments);
    }

    public async Task<string> GenerateAst(string fileName)
    {
        return await Execute("generateAst", new object[] {fileName});
    }

    public async Task<string> GenerateCode(object[] arguments)
    {
        return await Execute("generateCode", arguments);
    }
}