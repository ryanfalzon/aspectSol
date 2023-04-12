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
        host.Services.GetRequiredService<Program>().Run();
    }

    private void Run()
    {
        var script = @"
aspect SafeReenentrancy {
	add-to-declaration * ¬{ 
		bool private running = false; 
	}¬

	before execution-of *.* ¬{ 
		require (!running); 
	}¬
	
	before call-to *.setBalance() ¬{ 
		running = true; 
	}¬
	
	after call-to *.setBalance() ¬{ 
		running = false; 
	}¬
}";
        
        var smartContract = @"
pragma solidity >=0.7.0 <0.9.0;

contract CustomToken {

    uint256 private balance;

    function getBalance() public view returns(uint256) {
        return balance;
    }

    function setBalance(uint256 b) public {
        balance = b;
    }

    function placeBet(uint256 amount) public {
        uint256 newBalance = balance - amount;
        setBalance(newBalance);
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