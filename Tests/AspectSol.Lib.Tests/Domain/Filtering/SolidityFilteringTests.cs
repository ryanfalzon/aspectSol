using AspectSol.Lib.Domain.JavascriptExecution;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace AspectSol.Lib.Tests.Domain.Filtering;

public class SolidityFilteringTests
{
    protected const string WildcardToken = "*";

    protected readonly JToken ParsedContract;

    protected SolidityFilteringTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddNodeJS();
        serviceCollection.Configure<NodeJSProcessOptions>(options => options.ProjectPath = AppDomain.CurrentDomain.BaseDirectory);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var nodeJsService = serviceProvider.GetRequiredService<INodeJSService>();
        var scriptFactory = new ScriptFactory();

        var javascriptExecutor = new JavascriptExecutor(nodeJsService, scriptFactory);
        var smartContract = File.ReadAllTextAsync("Resources/SampleSmartContract.sol").Result;
        var result = javascriptExecutor.Execute("GenerateAST", new object[] { smartContract }).Result;

        ParsedContract = JToken.Parse(result);
    }
}