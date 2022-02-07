using AspectSol.Lib.Domain.JavascriptExecution;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.IO;
using Processors = AspectSol.Lib.Domain.Filtering.Solidity;

namespace AspectSol.Lib.Tests.Domain.Filtering.Solidity
{
    [TestFixture]
    public class ContractFilteringTests
    {
        private const string WildcardToken = "*";
        private const int TotalContracts = 2;
        private const int TotalContractsImplementingInterfaces = 1;

        private readonly JToken _parsedContract;

        public ContractFilteringTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddNodeJS();
            serviceCollection.Configure<NodeJSProcessOptions>(options => options.ProjectPath = AppDomain.CurrentDomain.BaseDirectory);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var nodeJsService = serviceProvider.GetRequiredService<INodeJSService>();
            var scriptFactory = new ScriptFactory();

            var javascriptExecutor = new JavascriptExecutor(nodeJsService, scriptFactory);
            var smartContract = File.ReadAllTextAsync("Resources/SampleSmartContract.sol").Result;
            var result = javascriptExecutor.Execute("GenerateAST", new object[] { smartContract }).Result;

            _parsedContract = JToken.Parse(result);
        }

        [Test]
        [TestCase("StorageA")]
        public void FilterContractsByContractName(string contractName)
        {
            var transformer = new Processors.ContractFiltering();
            var result = transformer.FilterContractsByContractName(_parsedContract, contractName);

            Assert.AreEqual(1, result.InterestedContracts.Count);
        }

        [Test]
        public void FilterContractsByContractNameWildcard()
        {
            var transformer = new Processors.ContractFiltering();
            var result = transformer.FilterContractsByContractName(_parsedContract, WildcardToken);

            Assert.AreEqual(TotalContracts, result.InterestedContracts.Count);
        }

        [Test]
        public void FilterContractsByContractNameThrowsError()
        {
            var transformer = new Processors.ContractFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterContractsByContractName(_parsedContract, null));
        }

        [Test]
        [TestCase("Space")]
        public void FilterContractsByInterfaceName(string interfaceName)
        {
            var transformer = new Processors.ContractFiltering();
            var result = transformer.FilterContractsByInterfaceName(_parsedContract, interfaceName);

            Assert.AreEqual(1, result.InterestedContracts.Count);
        }

        [Test]
        public void FilterContractsByInterfaceNameWildcard()
        {
            var transformer = new Processors.ContractFiltering();
            var result = transformer.FilterContractsByInterfaceName(_parsedContract, WildcardToken);

            Assert.AreEqual(TotalContractsImplementingInterfaces, result.InterestedContracts.Count);
        }

        [Test]
        public void FilterContractsByInterfaceNameThrowsError()
        {
            var transformer = new Processors.ContractFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterContractsByInterfaceName(_parsedContract, null));
        }
    }
}