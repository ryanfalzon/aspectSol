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
            var result = transformer.FilterContractsByContractName(ContractAst, contractName);

            Assert.AreEqual(1, result.InterestedContracts.Count);
        }

        [Test]
        public void FilterContractsByContractNameWildcard()
        {
            var transformer = new Processors.ContractFiltering();
            var result = transformer.FilterContractsByContractName(ContractAst, WildcardToken);

            Assert.AreEqual(TotalContracts, result.InterestedContracts.Count);
        }

        [Test]
        public void FilterContractsByContractNameThrowsError()
        {
            var transformer = new Processors.ContractFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterContractsByContractName(ContractAst, null));
        }

        [Test]
        [TestCase("Space")]
        public void FilterContractsByInterfaceName(string interfaceName)
        {
            var transformer = new Processors.ContractFiltering();
            var result = transformer.FilterContractsByInterfaceName(ContractAst, interfaceName);

            Assert.AreEqual(1, result.InterestedContracts.Count);
        }

        [Test]
        public void FilterContractsByInterfaceNameWildcard()
        {
            var transformer = new Processors.ContractFiltering();
            var result = transformer.FilterContractsByInterfaceName(ContractAst, WildcardToken);

            Assert.AreEqual(TotalContractsImplementingInterfaces, result.InterestedContracts.Count);
        }

        [Test]
        public void FilterContractsByInterfaceNameThrowsError()
        {
            var transformer = new Processors.ContractFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterContractsByInterfaceName(ContractAst, null));
        }
    }
}