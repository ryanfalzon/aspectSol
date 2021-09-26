using AspectSol.Compiler.App.SolidityProcessors;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace AspectSol.CLI
{
    public class Program
    {
        private static readonly int ExpectedParameterCount = 1;

        public static void Main(string[] args)
        {
            if(args.Length != ExpectedParameterCount)
            {
                throw new ArgumentOutOfRangeException($"Invalid number of arguments passed. Excpected={ExpectedParameterCount} | Actual={args.Length}");
            }

            var solidityFilePath = args[0];

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddNodeJS();
            serviceCollection.Configure<NodeJSProcessOptions>(options => options.ProjectPath = AppDomain.CurrentDomain.BaseDirectory);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var nodeJSService = serviceProvider.GetRequiredService<INodeJSService>();

            SolidityParser solidityParser = new SolidityParser(nodeJSService);
            var result = solidityParser.Parse(File.ReadAllText(solidityFilePath)).Result;

            SolidityTransformer transformer = new SolidityTransformer();
            transformer.FilterContractsByContractName(JToken.Parse(result), "Test");

            Console.WriteLine(result);
        }
    }
}