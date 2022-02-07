using NUnit.Framework;
using System;
using Processors = AspectSol.Lib.Domain.Filtering.Solidity;

namespace AspectSol.Lib.Tests.Domain.Filtering.Solidity
{
    [TestFixture]
    public class ContractFilteringTests : SolidityFilteringTests
    {
        private const int TotalContracts = 2;
        private const int TotalContractsImplementingInterfaces = 1;

        [Test]
        [TestCase("StorageA")]
        public void FilterContractsByContractName(string contractName)
        {
            var transformer = new Processors.ContractFiltering();
            var result = transformer.FilterContractsByContractName(ParsedContract, contractName);

            Assert.AreEqual(1, result.InterestedContracts.Count);
        }

        [Test]
        public void FilterContractsByContractNameWildcard()
        {
            var transformer = new Processors.ContractFiltering();
            var result = transformer.FilterContractsByContractName(ParsedContract, WildcardToken);

            Assert.AreEqual(TotalContracts, result.InterestedContracts.Count);
        }

        [Test]
        public void FilterContractsByContractNameThrowsError()
        {
            var transformer = new Processors.ContractFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterContractsByContractName(ParsedContract, null));
        }

        [Test]
        [TestCase("Space")]
        public void FilterContractsByInterfaceName(string interfaceName)
        {
            var transformer = new Processors.ContractFiltering();
            var result = transformer.FilterContractsByInterfaceName(ParsedContract, interfaceName);

            Assert.AreEqual(1, result.InterestedContracts.Count);
        }

        [Test]
        public void FilterContractsByInterfaceNameWildcard()
        {
            var transformer = new Processors.ContractFiltering();
            var result = transformer.FilterContractsByInterfaceName(ParsedContract, WildcardToken);

            Assert.AreEqual(TotalContractsImplementingInterfaces, result.InterestedContracts.Count);
        }

        [Test]
        public void FilterContractsByInterfaceNameThrowsError()
        {
            var transformer = new Processors.ContractFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterContractsByInterfaceName(ParsedContract, null));
        }
    }
}