using AspectSol.Compiler.Domain;
using Jering.Javascript.NodeJS;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AspectSol.Compiler.App.SolidityProcessors
{
    public class SolidityParser : IContractParser
    {
        private readonly INodeJSService nodeJSService;

        public SolidityParser(INodeJSService nodeJSService)
        {
            this.nodeJSService = nodeJSService ?? throw new ArgumentNullException(nameof(nodeJSService));
        }

        public async Task<string> Parse(string input)
        {
            string module = await File.ReadAllTextAsync("Infra/GenerateAST.js");
            return await nodeJSService.InvokeFromStringAsync<string>(module, args: new object[] { input });
        }
    }
}