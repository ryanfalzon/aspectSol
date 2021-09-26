using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.IO;
using Processors = AspectSol.Compiler.App.SolidityProcessors;

namespace AspectSol.Compiler.Tests.SolidityProcessors.SolidityTransformer
{
    [TestFixture]
    public class ContractFilteringTests
    {
        private const string wildcardToken = "*";
        private const int TotalContracts = 2;
        private const int TotalContractsImplementingInterfaces = 1;

        private readonly JToken parsedContract;

        public ContractFilteringTests()
        {
            parsedContract = JToken.Parse(File.ReadAllText("Resources/SmartContractAST.json"));
        }

        [Test]
        public void FilterContractsByContractName()
        {
            var transformer = new Processors.SolidityTransformer();
            var result = transformer.FilterContractsByContractName(parsedContract, "Election");

            Assert.AreEqual(1, result.InterestedContracts.Count);
        }

        [Test]
        public void FilterContractsByContractNameWildcard()
        {
            var transformer = new Processors.SolidityTransformer();
            var result = transformer.FilterContractsByContractName(parsedContract, wildcardToken);

            Assert.AreEqual(TotalContracts, result.InterestedContracts.Count);
        }

        [Test]
        public void FilterContractsByContractNameThrowsError()
        {
            var transformer = new Processors.SolidityTransformer();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterContractsByContractName(parsedContract, null));
        }

        [Test]
        public void FilterContractsByInterfaceName()
        {
            var transformer = new Processors.SolidityTransformer();
            var result = transformer.FilterContractsByContractName(parsedContract, "Election");

            Assert.AreEqual(1, result.InterestedContracts.Count);
        }

        [Test]
        public void FilterContractsByInterfaceNameWildcard()
        {
            var transformer = new Processors.SolidityTransformer();
            var result = transformer.FilterContractsByInterfaceName(parsedContract, wildcardToken);

            Assert.AreEqual(TotalContractsImplementingInterfaces, result.InterestedContracts.Count);
        }

        [Test]
        public void FilterContractsByInterfaceNameThrowsError()
        {
            var transformer = new Processors.SolidityTransformer();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterContractsByInterfaceName(parsedContract, null));
        }
    }
}