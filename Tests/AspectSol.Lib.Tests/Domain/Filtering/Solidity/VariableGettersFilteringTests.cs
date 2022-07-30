// using System;
// using AspectSol.Lib.Domain.Filtering.Solidity;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
//
// namespace AspectSol.Lib.Tests.Domain.Filtering.Solidity;
//
// [TestClass]
// public class VariableGettersFilteringTests : SolidityFilteringTests
// {
//     [TestMethod]
//     [DataRow("numberB", 14)]
//     public void FilterVariableGettersByVariableName(string variableName, int expectedCount)
//     {
//         var transformer = new VariableGettersFiltering();
//
//         var result1 = transformer.FilterVariableInteractionByVariableName(ContractAst, variableName);
//         Assert.AreEqual(expectedCount, result1.InterestedStatements.Count);
//     }
//
//     [TestMethod]
//     public void FilterVariableGettersByVariableNameWildcard()
//     {
//         var transformer = new VariableGettersFiltering();
//
//         var result = transformer.FilterVariableInteractionByVariableName(ContractAst, WildcardToken);
//         Assert.AreEqual(21, result.InterestedStatements.Count);
//     }
//
//     [TestMethod]
//     public void FilterVariableGettersByVariableNameThrowsError()
//     {
//         var transformer = new VariableGettersFiltering();
//         Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterVariableInteractionByVariableName(ContractAst, null));
//     }
//
//     [TestMethod]
//     [DataRow("uint256", 15)]
//     public void FilterVariableGettersByVariableType(string variableType, int expectedCount)
//     {
//         var transformer = new VariableGettersFiltering();
//         transformer.LoadLocals(ContractAst);
//
//         var result1 = transformer.FilterVariableInteractionByVariableType(ContractAst, variableType);
//         Assert.AreEqual(expectedCount, result1.InterestedStatements.Count);
//     }
//
//     [TestMethod]
//     public void FilterVariableGettersByVariableTypeWildcard()
//     {
//         var transformer = new VariableGettersFiltering();
//         transformer.LoadLocals(ContractAst);
//
//         var result = transformer.FilterVariableInteractionByVariableType(ContractAst, WildcardToken);
//         Assert.AreEqual(21, result.InterestedStatements.Count);
//     }
//
//     [TestMethod]
//     public void FilterVariableGettersByVariableTypeThrowsError()
//     {
//         var transformer = new VariableGettersFiltering();
//         Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterVariableInteractionByVariableType(ContractAst, null));
//     }
//
//     [TestMethod]
//     [DataRow("public", 13)]
//     public void FilterVariableGettersByVariableVisibility(string variableVisibility, int expectedCount)
//     {
//         var transformer = new VariableGettersFiltering();
//         transformer.LoadLocals(ContractAst);
//
//         var result1 = transformer.FilterVariableInteractionByVariableVisibility(ContractAst, variableVisibility);
//         Assert.AreEqual(expectedCount, result1.InterestedStatements.Count);
//     }
//
//     [TestMethod]
//     public void FilterVariableGettersByVariableVisibilityWildcard()
//     {
//         var transformer = new VariableGettersFiltering();
//         transformer.LoadLocals(ContractAst);
//
//         var result = transformer.FilterVariableInteractionByVariableVisibility(ContractAst, WildcardToken);
//         Assert.AreEqual(21, result.InterestedStatements.Count);
//     }
//
//     [TestMethod]
//     public void FilterVariableGettersByVariableVisibilityThrowsError()
//     {
//         var transformer = new VariableGettersFiltering();
//         Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterVariableInteractionByVariableVisibility(ContractAst, null));
//     }
// }