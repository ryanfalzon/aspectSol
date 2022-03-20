using AspectSol.Lib.Domain.JavascriptExecution;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Tests.Domain.Filtering;

public abstract class SolidityFilteringTests
{
    protected const string WildcardToken = "*";

    protected readonly JToken ParsedContract;
    protected readonly JToken ContractAst;

    protected SolidityFilteringTests()
    {
        var services = new ServiceCollection();

        services.AddNodeJS();
        services.AddScoped<ScriptFactory>();
        services.AddScoped<IJavascriptExecutor, JavascriptExecutor>();

        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetService<IJavascriptExecutor>();

        var response = service?.Execute("generateAst", new object[] { "Resources/SampleSmartContract.sol" }).Result;
        var deserializedResponse = JsonConvert.DeserializeObject<JavascriptResponse>(response ?? string.Empty);

        ParsedContract = JToken.Parse(deserializedResponse?.Data ?? string.Empty);
        ContractAst = ParsedContract["sources"]?.First?.First?["ast"];
    }
}