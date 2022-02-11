using NUnit.Framework;
using System;
using Processors = AspectSol.Lib.Domain.Filtering.Solidity;

namespace AspectSol.Lib.Tests.Domain.Filtering.Solidity
{
    public class VariableSettersFilteringTests : SolidityFilteringTests
    {
        [Test]
        [TestCase("numberB", 2)]
        public void FilterVariableSettersByVariableName(string variableName, int expectedCount)
        {
            var transformer = new Processors.VariableSettersFiltering();

            var result = transformer.FilterVariableInteractionByVariableName(ParsedContract, variableName);
            Assert.AreEqual(expectedCount, result.InterestedStatements.Count);
        }

        [Test]
        public void FilterVariableSettersByVariableNameWildcard()
        {
            var transformer = new Processors.VariableSettersFiltering();

            var result = transformer.FilterVariableInteractionByVariableName(ParsedContract, WildcardToken);
            Assert.AreEqual(11, result.InterestedStatements.Count);
        }

        [Test]
        public void FilterVariableSettersByVariableNameThrowsError()
        {
            var transformer = new Processors.VariableSettersFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterVariableInteractionByVariableName(ParsedContract, null));
        }

        [Test]
        [TestCase("uint256", 3)]
        public void FilterVariableSettersByVariableType(string variableType, int expectedCount)
        {
            var transformer = new Processors.VariableSettersFiltering();
            transformer.LoadLocals(ParsedContract);

            var result = transformer.FilterVariableInteractionByVariableType(ParsedContract, variableType);
            Assert.AreEqual(expectedCount, result.InterestedStatements.Count);
        }

        [Test]
        public void FilterVariableSettersByVariableTypeWildcard()
        {
            var transformer = new Processors.VariableSettersFiltering();
            transformer.LoadLocals(ParsedContract);

            var result = transformer.FilterVariableInteractionByVariableType(ParsedContract, WildcardToken);
            Assert.AreEqual(11, result.InterestedStatements.Count);
        }

        [Test]
        public void FilterVariableSettersByVariableTypeThrowsError()
        {
            var transformer = new Processors.VariableSettersFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterVariableInteractionByVariableType(ParsedContract, null));
        }

        [Test]
        [TestCase("public", 2)]
        public void FilterVariableSettersByVariableVisibility(string variableVisibility, int expectedCount)
        {
            var transformer = new Processors.VariableSettersFiltering();
            transformer.LoadLocals(ParsedContract);

            var result = transformer.FilterVariableInteractionByVariableVisibility(ParsedContract, variableVisibility);
            Assert.AreEqual(expectedCount, result.InterestedStatements.Count);
        }

        [Test]
        public void FilterVariableSettersByVariableVisibilityWildcard()
        {
            var transformer = new Processors.VariableSettersFiltering();
            transformer.LoadLocals(ParsedContract);

            var result = transformer.FilterVariableInteractionByVariableVisibility(ParsedContract, WildcardToken);
            Assert.AreEqual(11, result.InterestedStatements.Count);
        }

        [Test]
        public void FilterVariableSettersByVariableVisibilityThrowsError()
        {
            var transformer = new Processors.VariableSettersFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterVariableInteractionByVariableVisibility(ParsedContract, null));
        }
    }
}