using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspectSol.App;

public class Program
{
    private readonly ILogger<Program> _logger;

    public Program(ILogger<Program> logger)
    {
        _logger = logger;
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

        var command = args[0];

        switch (command)
        {
            case "execute" when args.Length == 4:
                {
                    var aspectSolFilePath = args[1];
                    var smartContractFilePath = args[2];
                    var outputFilePath = args[3];


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
            });
    }
}