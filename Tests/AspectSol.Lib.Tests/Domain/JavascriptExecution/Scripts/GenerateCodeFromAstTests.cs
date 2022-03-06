using AspectSol.Lib.Domain.JavascriptExecution;
using AspectSol.Lib.Tests.Domain.Filtering;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using System;

namespace AspectSol.Lib.Tests.Domain.JavascriptExecution.Scripts;

[TestFixture]
public class GenerateCodeFromAstTests : SolidityFilteringTests
{
    [Test]
    public void TestGenerateCodeReturnsSolidityCode()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddNodeJS();
        serviceCollection.Configure<NodeJSProcessOptions>(options => options.ProjectPath = AppDomain.CurrentDomain.BaseDirectory);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var nodeJsService = serviceProvider.GetRequiredService<INodeJSService>();
        var scriptFactory = new ScriptFactory();

        var javascriptExecutor = new JavascriptExecutor(nodeJsService, scriptFactory);

        var generateAstResponse = javascriptExecutor.Execute("generateAst", new object[] { "Resources/SampleSmartContract.sol" }).Result;
        var deserializedGenerateAstResponse = JsonConvert.DeserializeObject<JavascriptResponse>(generateAstResponse);

        var sourceCode = javascriptExecutor.Execute("generateCode", new object[] { deserializedGenerateAstResponse.Data }).Result;

        Assert.IsNotNull(sourceCode);
    }
}