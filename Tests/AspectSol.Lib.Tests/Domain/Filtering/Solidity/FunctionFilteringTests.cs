using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Processors = AspectSol.Lib.Domain.Filtering.Solidity;

namespace AspectSol.Lib.Tests.Domain.Filtering.Solidity
{
    [TestFixture]
    public class FunctionFilteringTests : SolidityFilteringTests
    {
        private const int TotalFunctions = 7;

        [Test]
        [TestCase("store", 1)]
        public void FilterFunctionsByFunctionName(string functionName, int expectedCount)
        {
            var transformer = new Processors.FunctionFiltering();
            var result = transformer.FilterFunctionsByFunctionName(ParsedContract, functionName);

            Assert.AreEqual(expectedCount, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByFunctionNameWildcard()
        {
            var transformer = new Processors.FunctionFiltering();
            var result = transformer.FilterFunctionsByFunctionName(ParsedContract, WildcardToken);

            Assert.AreEqual(TotalFunctions, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByFunctionNameThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByFunctionName(ParsedContract, null));
        }

        [Test]
        [TestCase("private", 3)]
        [TestCase("public", 4)]
        public void FilterFunctionsByVisibility(string visibility, int expectedCount)
        {
            var transformer = new Processors.FunctionFiltering();

            var result = transformer.FilterFunctionsByVisibility(ParsedContract, visibility);
            Assert.AreEqual(expectedCount, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByVisibilityWildcard()
        {
            var transformer = new Processors.FunctionFiltering();
            var result = transformer.FilterFunctionsByVisibility(ParsedContract, WildcardToken);

            Assert.AreEqual(TotalFunctions, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByVisibilityThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByVisibility(ParsedContract, null));
        }

        [Test]
        [TestCase("pure", 2)]
        public void FilterFunctionsByStateMutability(string mutability, int expectedCount)
        {
            var transformer = new Processors.FunctionFiltering();
            var result = transformer.FilterFunctionsByStateMutability(ParsedContract, mutability);

            Assert.AreEqual(expectedCount, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByStateMutabilityWildcard()
        {
            var transformer = new Processors.FunctionFiltering();
            var result = transformer.FilterFunctionsByStateMutability(ParsedContract, WildcardToken);

            Assert.AreEqual(TotalFunctions, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByStateMutabilityThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByStateMutability(ParsedContract, null));
        }

        [Test]
        [TestCase(1, "onlyOwner")]
        [TestCase(0, "invalidModifier")]
        public void FilterFunctionsByAllModifiers(int expectedCount, params string[] modifiers)
        {
            var transformer = new Processors.FunctionFiltering();

            var result = transformer.FilterFunctionsByAllModifiers(ParsedContract, modifiers.ToList());
            Assert.AreEqual(expectedCount, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByAllModifiersThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByAllModifiers(ParsedContract, null));
        }

        [Test]
        [TestCase(1, "onlyOwner", "onlyAdmins")]
        [TestCase(0, "invalidModifier")]
        public void FilterFunctionsByEitherModifiers(int expectedCount, params string[] modifiers)
        {
            var transformer = new Processors.FunctionFiltering();

            var result = transformer.FilterFunctionsByEitherModifiers(ParsedContract, modifiers.ToList());
            Assert.AreEqual(expectedCount, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByEitherModifiersThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByEitherModifiers(ParsedContract, null));
        }

        [Test]
        [TestCase(1, "onlyOwner", false)]
        [TestCase(6, "onlyOwner", true)]
        public void FilterFunctionsByModifier(int expectedCount, string modifier, bool invert)
        {
            var transformer = new Processors.FunctionFiltering();

            var result = transformer.FilterFunctionsByModifier(ParsedContract, modifier, invert);
            Assert.AreEqual(expectedCount, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByModifierThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByModifier(ParsedContract, null, false));
        }

        [Test]
        public void FilterFunctionsByParameters()
        {
            var transformer = new Processors.FunctionFiltering();

            var result1 = transformer.FilterFunctionsByParameters(ParsedContract, "uint256", "num");
            Assert.AreEqual(2, result1.InterestedFunctions.Count);

            var result2 = transformer.FilterFunctionsByParameters(ParsedContract, new List<(string Type, string Value)> { ("uint256", "num") });
            Assert.AreEqual(2, result2.InterestedFunctions.Count);

            var result3 = transformer.FilterFunctionsByParameters(ParsedContract, new List<(string Type, string Value)> { ("uint128", "anotherNum") });
            Assert.AreEqual(0, result3.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByParametersThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByParameters(ParsedContract, null, "num"));
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByParameters(ParsedContract, "uint256", null));
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByParameters(ParsedContract, null));
        }

        [Test]
        public void FilterFunctionsByReturnParameters()
        {
            var transformer = new Processors.FunctionFiltering();

            var result1 = transformer.FilterFunctionsByReturnParameters(ParsedContract, "address");
            Assert.AreEqual(0, result1.InterestedFunctions.Count);

            var result2 = transformer.FilterFunctionsByReturnParameters(ParsedContract, "uint256");
            Assert.AreEqual(4, result2.InterestedFunctions.Count);

            var result3 = transformer.FilterFunctionsByReturnParameters(ParsedContract, new List<string> { "bool" });
            Assert.AreEqual(1, result3.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByReturnParametersThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByReturnParameters(ParsedContract, returnParameter: null));
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByReturnParameters(ParsedContract, returnParameters: null));
        }

        [Test]
        [TestCase("storageA", "store", 1)]
        [TestCase("storageA", "someOtherFunction", 0)]
        public void FilterFunctionCallsByInstanceName(string instanceName, string functionName, int expectedCount)
        {
            var transformer = new Processors.FunctionFiltering();

            var result = transformer.FilterFunctionCallsByInstanceName(ParsedContract, instanceName, functionName);
            Assert.AreEqual(expectedCount, result.InterestedStatements.Count);
        }

        [Test]
        [TestCase(null, "store")]
        [TestCase("storageA", null)]
        public void FilterFunctionCallsByInstanceNameThrowsError(string instanceName, string functionName)
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionCallsByInstanceName(ParsedContract, instanceName, functionName));
        }

        [Test]
        [TestCase("Space")]
        public void FilterFunctionsImplementedFromInterface(string interfaceName)
        {
            var transformer = new Processors.FunctionFiltering();

            var result1 = transformer.FilterFunctionsImplementedFromInterface(ParsedContract, interfaceName, false);
            Assert.AreEqual(2, result1.InterestedFunctions.Count);

            var result2 = transformer.FilterFunctionsImplementedFromInterface(ParsedContract, interfaceName, true);
            Assert.AreEqual(5, result2.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsImplementedFromInterfaceThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsImplementedFromInterface(ParsedContract, null, false));
        }
    }
}