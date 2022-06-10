using AspectSol.Lib.App;
using AspectSol.Lib.Domain.JavascriptExecution;
using AspectSol.Lib.Infra.ServiceCollectionExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AspectSol.App;

public class Program
{
    private readonly ILogger<Program> _logger;
    private readonly AppService _appService;
    private readonly IJavascriptExecutor _javascriptExecutor;

    public Program(ILogger<Program> logger, AppService appService, IJavascriptExecutor javascriptExecutor)
    {
        _logger = logger;
        _appService = appService;
        _javascriptExecutor = javascriptExecutor;
    }

    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        host.Services.GetRequiredService<Program>().Run(args);
    }

    public void Run(string[] args)
    {
        if (args.Length == 0)
        {
            _logger.LogError("Invalid arguments");
        }

        var ast = _javascriptExecutor.Execute("generateAst", new object[] { "Resources/SampleSmartContract.sol" }).Result;
        var code = _javascriptExecutor.Execute("generateCode", new object[] { JsonConvert.DeserializeObject<JavascriptResponse>(ast)?.Data }).Result;

        //var command = args[0];

        //switch (command)
        //{
        //    case "execute" when args.Length == 4:
        //        {
        //            //var aspectSolFilePath = args[1];
        //            //var smartContractFilePath = args[2];
        //            //var outputFilePath = args[3];

        //            //await _appService.Execute(aspectSolFilePath, smartContractFilePath, outputFilePath);

        //            var ast = _javascriptExecutor.Execute("generateAst", new object[] { "Resources/SampleSmartContract.sol" }).Result;
        //            var code = _javascriptExecutor.Execute("generateCode", new object[] { JsonConvert.DeserializeObject<JavascriptResponse>(ast)?.Data }).Result;
        //        }
        //        break;

        //}
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddTransient<Program>();
                AspectSolCollectionExtensions.AddAspectSolServiceConfig(services);
            });
    }
}