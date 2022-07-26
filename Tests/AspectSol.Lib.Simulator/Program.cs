using AspectSol.Lib.App;
using AspectSol.Lib.Infra.ServiceCollectionExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspectSol.Lib.Simulator;

public class Program
{
    private readonly AppService _appService;

    public Program(AppService appService)
    {
        _appService = appService;
    }

    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        host.Services.GetRequiredService<Program>().RunAsync();
    }

    private void RunAsync()
    {
        var script = @"
aspect SafeReenentrancy {
	add-to-declaration * ¬{ 
		bool private running = false; 
	}¬

	before execution-of *.* ¬{ 
		require (!running); 
	}¬
}";
        
        var smartContract = @"
pragma solidity >=0.7.0 <0.9.0;

contract CustomToken {

    uint256 private balance;

    function getBalance() public view returns(uint256) {
        return balance;
    }

    function setbalance(uint256 b) public {
        balance = b;
    }
}";
        
        var newSmartContract = _appService.Execute(script, smartContract);
        Console.WriteLine(newSmartContract);
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