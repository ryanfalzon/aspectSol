using AspectSol.Lib.App;
using AspectSol.Lib.Infra;
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
        _logger = logger;
        _appService = appService;
    }

    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        host.Services.GetRequiredService<Program>().Run(args).RunSynchronously();
    }

    public async Task Run(string[] args)
    {
        if (args.Length == 0)
        {
            _logger.LogError("Invalid arguments");
        }

        var command = args[0];

        switch (command)
        {
            case "execute" when args.Length == 4:
                {
                    var aspectSolFilePath = args[1];
                    var smartContractFilePath = args[2];
                    var outputFilePath = args[3];

                    await _appService.Execute(aspectSolFilePath, smartContractFilePath, outputFilePath);
                }
                break;

        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddTransient<Program>();
                services.AddAspectSolServiceConfig();
            });
    }
}