using AspectSol.Lib.Domain.JavascriptExecution;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace AspectSol.Lib.Tests.Domain.JavascriptExecution;

[TestClass]
public class JavascriptExecutorTests
{
    [TestMethod]
    public void GenerateCodeShouldRunSuccessfully()
    {
        var services = new ServiceCollection();

        services.AddNodeJS();
        services.AddScoped<ScriptFactory>();
        services.AddScoped<IJavascriptExecutor, JavascriptExecutor>();

        var serviceProvider = services.BuildServiceProvider();

        var service = serviceProvider.GetService<IJavascriptExecutor>();
        var ast = service?.Execute("generateAst", new object[] { "Resources/SampleSmartContract.sol" }).Result;
        var code = service?.Execute("generateCode", new object[] { JsonConvert.DeserializeObject<JavascriptResponse>(ast)?.Data }).Result;

        Assert.IsNotNull(code);
    }
}