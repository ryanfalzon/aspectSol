// using System;
// using AspectSol.Lib.Domain.Filtering.Solidity;
// using Microsoft.VisualStudio.TestTools.UnitTesting;
//
// namespace AspectSol.Lib.Tests.Domain.Filtering.Solidity;
//
// [TestClass]
// public class VariableSettersFilteringTests : SolidityFilteringTests
// {
//     [TestMethod]
//     [DataRow("numberB", 2)]
//     public void FilterVariableSettersByVariableName(string variableName, int expectedCount)
//     {
//         var transformer = new VariableSettersFiltering();
//
//         var result = transformer.FilterVariableInteractionByVariableName(ContractAst, variableName);
//         Assert.AreEqual(expectedCount, result.InterestedStatements.Count);
//     }
//
//     [TestMethod]
//     public void FilterVariableSettersByVariableNameWildcard()
//     {
//         var transformer = new VariableSettersFiltering();
//
//         var result = transformer.FilterVariableInteractionByVariableName(ContractAst, WildcardToken);
//         Assert.AreEqual(12, result.InterestedStatements.Count);
//     }
//
//     [TestMethod]
//     public void FilterVariableSettersByVariableNameThrowsExceptionError()
//     {
//         var transformer = new VariableSettersFiltering();
//         Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterVariableInteractionByVariableName(ContractAst, null));
//     }
//
//     [TestMethod]
//     [DataRow("uint256", 3)]
//     public void FilterVariableSettersByVariableType(string variableType, int expectedCount)
//     {
//         var transformer = new VariableSettersFiltering();
//         transformer.LoadLocals(ContractAst);
//
//         var result = transformer.FilterVariableInteractionByVariableType(ContractAst, variableType);
//         Assert.AreEqual(expectedCount, result.InterestedStatements.Count);
//     }
//
//     [TestMethod]
//     public void FilterVariableSettersByVariableTypeWildcard()
//     {
//         var transformer = new VariableSettersFiltering();
//         transformer.LoadLocals(ContractAst);
//
//         var result = transformer.FilterVariableInteractionByVariableType(ContractAst, WildcardToken);
//         Assert.AreEqual(12, result.InterestedStatements.Count);
//     }
//
//     [TestMethod]
//     public void FilterVariableSettersByVariableTypeThrowsExceptionError()
//     {
//         var transformer = new VariableSettersFiltering();
//         Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterVariableInteractionByVariableType(ContractAst, null));
//     }
//
//     [TestMethod]
//     [DataRow("public", 2)]
//     public void FilterVariableSettersByVariableVisibility(string variableVisibility, int expectedCount)
//     {
//         var transformer = new VariableSettersFiltering();
//         transformer.LoadLocals(ContractAst);
//
//         var result = transformer.FilterVariableInteractionByVariableVisibility(ContractAst, variableVisibility);
//         Assert.AreEqual(expectedCount, result.InterestedStatements.Count);
//     }
//
//     [TestMethod]
//     public void FilterVariableSettersByVariableVisibilityWildcard()
//     {
//         var transformer = new VariableSettersFiltering();
//         transformer.LoadLocals(ContractAst);
//
//         var result = transformer.FilterVariableInteractionByVariableVisibility(ContractAst, WildcardToken);
//         Assert.AreEqual(12, result.InterestedStatements.Count);
//     }
//
//     [TestMethod]
//     public void FilterVariableSettersByVariableVisibilityThrowsExceptionError()
//     {
//         var transformer = new VariableSettersFiltering();
//         Assert.ThrowsException<ArgumentNullException>(() => transformer.FilterVariableInteractionByVariableVisibility(ContractAst, null));
//     }
// }