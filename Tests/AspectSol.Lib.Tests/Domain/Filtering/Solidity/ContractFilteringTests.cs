using AspectSol.Lib.Domain.Filtering.Solidity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AspectSol.Lib.Tests.Domain.Filtering.Solidity;

[TestClass]
public class ContractFilteringTests : SolidityFilteringTests
{
    private const int TotalContracts = 2;
    private const int TotalContractsImplementingInterfaces = 1;

    [TestMethod]
    [DataRow("StorageA")]
    public void FilterContractsByContractName(string contractName)
    {
        var transformer = new ContractFiltering();
        var result = transformer.FilterContractsByContractName(ContractAst, contractName);

        Assert.AreEqual(1, result.InterestedContracts.Count);
    }

    [TestMethod]
    public void FilterContractsByContractNameWildcard()
    {
        var transformer = new ContractFiltering();
        var result = transformer.FilterContractsByContractName(ContractAst, WildcardToken);

        Assert.AreEqual(TotalContracts, result.InterestedContracts.Count);
    }

    [TestMethod]
    public void FilterContractsByContractNameThrowsError()
    {
        var transformer = new ContractFiltering();
        Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterContractsByContractName(ContractAst, null));
    }

    [TestMethod]
    [DataRow("Space")]
    public void FilterContractsByInterfaceName(string interfaceName)
    {
        var transformer = new ContractFiltering();
        var result = transformer.FilterContractsByInterfaceName(ContractAst, interfaceName);

        Assert.AreEqual(1, result.InterestedContracts.Count);
    }

    [TestMethod]
    public void FilterContractsByInterfaceNameWildcard()
    {
        var transformer = new ContractFiltering();
        var result = transformer.FilterContractsByInterfaceName(ContractAst, WildcardToken);

        Assert.AreEqual(TotalContractsImplementingInterfaces, result.InterestedContracts.Count);
    }

    [TestMethod]
    public void FilterContractsByInterfaceNameThrowsError()
    {
        var transformer = new ContractFiltering();
        Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterContractsByInterfaceName(ContractAst, null));
    }
}