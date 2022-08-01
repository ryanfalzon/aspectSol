using AspectSol.Lib.App;
using AspectSol.Lib.Infra.ServiceCollectionExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspectSol.App;

public class Program
{
    private readonly ILogger<Program> _logger;
    private readonly AppService _appService;

    public Program(ILogger<Program> logger, AppService appService)
    {
        _logger     = logger;
        _appService = appService;
    }

    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        host.Services.GetRequiredService<Program>().Run(args);
    }

    private void Run(IReadOnlyList<string> args)
    {
        if (args.Count != 3)
        {
            _logger.LogError("Invalid arguments");
        }

        var scriptPath = args[0];
        var smartContractPath = args[1];
        var newSmartContractPath = args[2];
        
        if(!File.Exists(scriptPath)) _logger.LogError("Script file does not exist");
        else if(!File.Exists(smartContractPath)) _logger.LogError("Smart contract file does not exist");
        else
        {
            var script = File.ReadAllText(scriptPath);
            var smartContract = File.ReadAllText(smartContractPath);
            
            var result = _appService.Execute(script, smartContract);
            File.WriteAllText(newSmartContractPath, result);
            
            _logger.LogInformation("Process completed successfully");
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddTransient<Program>();
                services.AddAspectSolServiceConfig();
                services.AddSolidityFilteringConfig();
            });
    }
}