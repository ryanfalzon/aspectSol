using AspectSol.Lib.Domain.JavascriptExecution;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace AspectSol.Lib.Tests.Domain.Filtering;

public class SolidityFilteringTests
{
    protected const string WildcardToken = "*";

    protected readonly JToken ParsedContract;
    protected readonly JToken ContractAst;

    protected SolidityFilteringTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddNodeJS();
        serviceCollection.Configure<NodeJSProcessOptions>(options => options.ProjectPath = AppDomain.CurrentDomain.BaseDirectory);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var nodeJsService = serviceProvider.GetRequiredService<INodeJSService>();
        var scriptFactory = new ScriptFactory();

        var javascriptExecutor = new JavascriptExecutor(nodeJsService, scriptFactory);
        var response = javascriptExecutor.Execute("generateAst", new object[] { "Resources/SampleSmartContract.sol" }).Result;
        var deserializedResponse = JsonConvert.DeserializeObject<JavascriptResponse>(response);

        ParsedContract = JToken.Parse(deserializedResponse?.Data ?? string.Empty);
        ContractAst = ParsedContract["sources"]?.First?.First?["ast"];
    }
}