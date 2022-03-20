using AspectSol.Lib.Domain.Filtering.Solidity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspectSol.Lib.Tests.Domain.Filtering.Solidity;

[TestClass]
public class FunctionFilteringTests : SolidityFilteringTests
{
    private const int TotalFunctions = 7;

    [TestMethod]
    [DataRow("store", 1)]
    public void FilterFunctionsByFunctionName(string functionName, int expectedCount)
    {
        var transformer = new FunctionFiltering();
        var result = transformer.FilterFunctionsByFunctionName(ContractAst, functionName);

        Assert.AreEqual(expectedCount, result.InterestedFunctions.Count);
    }

    [TestMethod]
    public void FilterFunctionsByFunctionNameWildcard()
    {
        var transformer = new FunctionFiltering();
        var result = transformer.FilterFunctionsByFunctionName(ContractAst, WildcardToken);

        Assert.AreEqual(TotalFunctions, result.InterestedFunctions.Count);
    }

    [TestMethod]
    public void FilterFunctionsByFunctionNameThrowsError()
    {
        var transformer = new FunctionFiltering();
        Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterFunctionsByFunctionName(ContractAst, null));
    }

    [TestMethod]
    [DataRow("private", 3)]
    [DataRow("public", 4)]
    public void FilterFunctionsByVisibility(string visibility, int expectedCount)
    {
        var transformer = new FunctionFiltering();

        var result = transformer.FilterFunctionsByVisibility(ContractAst, visibility);
        Assert.AreEqual(expectedCount, result.InterestedFunctions.Count);
    }

    [TestMethod]
    public void FilterFunctionsByVisibilityWildcard()
    {
        var transformer = new FunctionFiltering();
        var result = transformer.FilterFunctionsByVisibility(ContractAst, WildcardToken);

        Assert.AreEqual(TotalFunctions, result.InterestedFunctions.Count);
    }

    [TestMethod]
    public void FilterFunctionsByVisibilityThrowsError()
    {
        var transformer = new FunctionFiltering();
        Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterFunctionsByVisibility(ContractAst, null));
    }

    [TestMethod]
    [DataRow("view", 3)]
    public void FilterFunctionsByStateMutability(string mutability, int expectedCount)
    {
        var transformer = new FunctionFiltering();
        var result = transformer.FilterFunctionsByStateMutability(ContractAst, mutability);

        Assert.AreEqual(expectedCount, result.InterestedFunctions.Count);
    }

    [TestMethod]
    public void FilterFunctionsByStateMutabilityWildcard()
    {
        var transformer = new FunctionFiltering();
        var result = transformer.FilterFunctionsByStateMutability(ContractAst, WildcardToken);

        Assert.AreEqual(TotalFunctions, result.InterestedFunctions.Count);
    }

    [TestMethod]
    public void FilterFunctionsByStateMutabilityThrowsError()
    {
        var transformer = new FunctionFiltering();
        Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterFunctionsByStateMutability(ContractAst, null));
    }

    [TestMethod]
    [DataRow(1, "onlyOwner")]
    [DataRow(0, "invalidModifier")]
    public void FilterFunctionsByAllModifiers(int expectedCount, params string[] modifiers)
    {
        var transformer = new FunctionFiltering();

        var result = transformer.FilterFunctionsByAllModifiers(ContractAst, modifiers.ToList());
        Assert.AreEqual(expectedCount, result.InterestedFunctions.Count);
    }

    [TestMethod]
    public void FilterFunctionsByAllModifiersThrowsError()
    {
        var transformer = new FunctionFiltering();
        Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterFunctionsByAllModifiers(ContractAst, null));
    }

    [TestMethod]
    [DataRow(1, "onlyOwner", "onlyAdmins")]
    [DataRow(0, "invalidModifier")]
    public void FilterFunctionsByEitherModifiers(int expectedCount, params string[] modifiers)
    {
        var transformer = new FunctionFiltering();

        var result = transformer.FilterFunctionsByEitherModifiers(ContractAst, modifiers.ToList());
        Assert.AreEqual(expectedCount, result.InterestedFunctions.Count);
    }

    [TestMethod]
    public void FilterFunctionsByEitherModifiersThrowsError()
    {
        var transformer = new FunctionFiltering();
        Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterFunctionsByEitherModifiers(ContractAst, null));
    }

    [TestMethod]
    [DataRow(1, "onlyOwner", false)]
    [DataRow(6, "onlyOwner", true)]
    public void FilterFunctionsByModifier(int expectedCount, string modifier, bool invert)
    {
        var transformer = new FunctionFiltering();

        var result = transformer.FilterFunctionsByModifier(ContractAst, modifier, invert);
        Assert.AreEqual(expectedCount, result.InterestedFunctions.Count);
    }

    [TestMethod]
    public void FilterFunctionsByModifierThrowsError()
    {
        var transformer = new FunctionFiltering();
        Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterFunctionsByModifier(ContractAst, null, false));
    }

    [TestMethod]
    public void FilterFunctionsByParameters()
    {
        var transformer = new FunctionFiltering();

        var result1 = transformer.FilterFunctionsByParameters(ContractAst, "uint256", "num");
        Assert.AreEqual(2, result1.InterestedFunctions.Count);

        var result2 = transformer.FilterFunctionsByParameters(ContractAst, new List<(string Type, string Value)> { ("uint256", "num") });
        Assert.AreEqual(2, result2.InterestedFunctions.Count);

        var result3 = transformer.FilterFunctionsByParameters(ContractAst, new List<(string Type, string Value)> { ("uint128", "anotherNum") });
        Assert.AreEqual(0, result3.InterestedFunctions.Count);
    }

    [TestMethod]
    public void FilterFunctionsByParametersThrowsError()
    {
        var transformer = new FunctionFiltering();
        Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterFunctionsByParameters(ContractAst, null, "num"));
        Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterFunctionsByParameters(ContractAst, "uint256", null));
        Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterFunctionsByParameters(ContractAst, null));
    }

    [TestMethod]
    public void FilterFunctionsByReturnParameters()
    {
        var transformer = new FunctionFiltering();

        var result1 = transformer.FilterFunctionsByReturnParameters(ContractAst, "address");
        Assert.AreEqual(0, result1.InterestedFunctions.Count);

        var result2 = transformer.FilterFunctionsByReturnParameters(ContractAst, "uint256");
        Assert.AreEqual(4, result2.InterestedFunctions.Count);

        var result3 = transformer.FilterFunctionsByReturnParameters(ContractAst, new List<string> { "bool" });
        Assert.AreEqual(1, result3.InterestedFunctions.Count);
    }

    [TestMethod]
    public void FilterFunctionsByReturnParametersThrowsError()
    {
        var transformer = new FunctionFiltering();
        Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterFunctionsByReturnParameters(ContractAst, returnParameter: null));
        Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterFunctionsByReturnParameters(ContractAst, returnParameters: null));
    }

    [TestMethod]
    [DataRow("storageA", "store", 1)]
    [DataRow("storageA", "someOtherFunction", 0)]
    public void FilterFunctionCallsByInstanceName(string instanceName, string functionName, int expectedCount)
    {
        var transformer = new FunctionFiltering();

        var result = transformer.FilterFunctionCallsByInstanceName(ContractAst, instanceName, functionName);
        Assert.AreEqual(expectedCount, result.InterestedStatements.Count);
    }

    [TestMethod]
    [DataRow(null, "store")]
    [DataRow("storageA", null)]
    public void FilterFunctionCallsByInstanceNameThrowsError(string instanceName, string functionName)
    {
        var transformer = new FunctionFiltering();
        Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterFunctionCallsByInstanceName(ContractAst, instanceName, functionName));
    }

    [TestMethod]
    [DataRow("Space")]
    public void FilterFunctionsImplementedFromInterface(string interfaceName)
    {
        var transformer = new FunctionFiltering();

        var result1 = transformer.FilterFunctionsImplementedFromInterface(ContractAst, interfaceName, false);
        Assert.AreEqual(2, result1.InterestedFunctions.Count);

        var result2 = transformer.FilterFunctionsImplementedFromInterface(ContractAst, interfaceName, true);
        Assert.AreEqual(5, result2.InterestedFunctions.Count);
    }

    [TestMethod]
    public void FilterFunctionsImplementedFromInterfaceThrowsError()
    {
        var transformer = new FunctionFiltering();
        Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterFunctionsImplementedFromInterface(ContractAst, null, false));
    }
}