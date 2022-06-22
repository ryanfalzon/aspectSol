using AspectSol.Lib.Domain.Parsing;
using AspectSol.Lib.Domain.Tokenization;
using AspectSol.Lib.Infra.ServiceCollectionExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspectSol.Lib.Simulator;

public class Program
{
    private readonly ITokenizer _tokenizer;
    private readonly IParser _parser;

    public Program(ITokenizer tokenizer, IParser parser)
    {
        _tokenizer = tokenizer;
        _parser    = parser;
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
		private bool running = false; 
	}¬

	before execution-of *.* ¬{ 
		require (!running); 
	}¬
	
	before call-to *.transfer() ¬{ 
		running = true; 
	}¬
	
	after call-to *.transfer() ¬{ 
		running = false; 
	}¬
}";

        var tokens = _tokenizer.Tokenize(script);
        var ast = _parser.Parse(tokens);
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