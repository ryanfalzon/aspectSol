using System;
using System.Linq;
using AspectSol.Lib.Domain.Filtering.Solidity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspectSol.Lib.Tests.Domain.Filtering.Solidity;

[TestClass]
public class VariableDefinitionFilteringTests : SolidityFilteringTests
{
    private const int TotalVariableDefinitions = 4;

    [TestMethod]
    [DataRow("address", 1)]
    public void FilterVariableDefinitionByVariableType(string variableType, int expectedCount)
    {
        var transformer = new VariableDefinitionFiltering();
        var result = transformer.FilterVariableDefinitionByVariableType(ContractAst, variableType);
        var interestedDefinitions = result.ContractFilteringResults.Sum(x => x.DefinitionFilteringResults.Count);

        Assert.AreEqual(expectedCount, interestedDefinitions);
    }

    [TestMethod]
    public void FilterVariableDefinitionByVariableTypeWildcard()
    {
        var transformer = new VariableDefinitionFiltering();
        var result = transformer.FilterVariableDefinitionByVariableType(ContractAst, WildcardToken);
        var interestedDefinitions = result.ContractFilteringResults.Sum(x => x.DefinitionFilteringResults.Count);

        Assert.AreEqual(TotalVariableDefinitions, interestedDefinitions);
    }

    [TestMethod]
    public void FilterVariableDefinitionByVariableTypeThrowsError()
    {
        var transformer = new VariableDefinitionFiltering();
        Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterVariableDefinitionByVariableType(ContractAst, null));
    }

    [TestMethod]
    [DataRow("someAddress", 1)]
    [DataRow("someOtherAddress", 0)]
    public void FilterVariableDefinitionByVariableName(string variableName, int expectedCount)
    {
        var transformer = new VariableDefinitionFiltering();
        var result = transformer.FilterVariableDefinitionByVariableName(ContractAst, variableName);
        var interestedDefinitions = result.ContractFilteringResults.Sum(x => x.DefinitionFilteringResults.Count);
        
        Assert.AreEqual(expectedCount, interestedDefinitions);
    }

    [TestMethod]
    public void FilterVariableDefinitionByVariableNameWildcard()
    {
        var transformer = new VariableDefinitionFiltering();
        var result = transformer.FilterVariableDefinitionByVariableName(ContractAst, WildcardToken);
        var interestedDefinitions = result.ContractFilteringResults.Sum(x => x.DefinitionFilteringResults.Count);

        Assert.AreEqual(TotalVariableDefinitions, interestedDefinitions);
    }

    [TestMethod]
    public void FilterVariableDefinitionByVariableNameThrowsError()
    {
        var transformer = new VariableDefinitionFiltering();
        Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterVariableDefinitionByVariableName(ContractAst, null));
    }
}