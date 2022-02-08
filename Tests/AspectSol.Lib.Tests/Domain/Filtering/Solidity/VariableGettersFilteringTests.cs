using NUnit.Framework;
using System;
using Processors = AspectSol.Lib.Domain.Filtering.Solidity;

namespace AspectSol.Lib.Tests.Domain.Filtering.Solidity
{
    public class VariableGettersFilteringTests : SolidityFilteringTests
    {
        [Test]
        [TestCase("numberB", 13)]
        public void FilterVariableGettersByVariableName(string variableName, int expectedCount)
        {
            var transformer = new Processors.VariableGettersFiltering();

            var result1 = transformer.FilterVariableGettersByVariableName(ParsedContract, variableName);
            Assert.AreEqual(expectedCount, result1.InterestedStatements.Count);
        }

        [Test]
        public void FilterVariableGettersByVariableNameWildcard()
        {
            var transformer = new Processors.VariableGettersFiltering();

            var result = transformer.FilterVariableGettersByVariableName(ParsedContract, WildcardToken);
            Assert.AreEqual(20, result.InterestedStatements.Count);
        }

        [Test]
        public void FilterVariableGettersByVariableNameThrowsError()
        {
            var transformer = new Processors.VariableGettersFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterVariableGettersByVariableName(ParsedContract, null));
        }

        [Test]
        [TestCase("uint256", 14)]
        public void FilterVariableGettersByVariableType(string variableType, int expectedCount)
        {
            var transformer = new Processors.VariableGettersFiltering();
            transformer.LoadLocals(ParsedContract);

            var result1 = transformer.FilterVariableGettersByVariableType(ParsedContract, variableType);
            Assert.AreEqual(expectedCount, result1.InterestedStatements.Count);
        }

        [Test]
        public void FilterVariableGettersByVariableTypeWildcard()
        {
            var transformer = new Processors.VariableGettersFiltering();
            transformer.LoadLocals(ParsedContract);

            var result = transformer.FilterVariableGettersByVariableType(ParsedContract, WildcardToken);
            Assert.AreEqual(20, result.InterestedStatements.Count);
        }

        [Test]
        public void FilterVariableGettersByVariableTypeThrowsError()
        {
            var transformer = new Processors.VariableGettersFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterVariableGettersByVariableType(ParsedContract, null));
        }

        [Test]
        [TestCase("public", 13)]
        public void FilterVariableGettersByVariableVisibility(string variableVisibility, int expectedCount)
        {
            var transformer = new Processors.VariableGettersFiltering();
            transformer.LoadLocals(ParsedContract);

            var result1 = transformer.FilterVariableGettersByVariableVisibility(ParsedContract, variableVisibility);
            Assert.AreEqual(expectedCount, result1.InterestedStatements.Count);
        }

        [Test]
        public void FilterVariableGettersByVariableVisibilityWildcard()
        {
            var transformer = new Processors.VariableGettersFiltering();
            transformer.LoadLocals(ParsedContract);

            var result = transformer.FilterVariableGettersByVariableVisibility(ParsedContract, WildcardToken);
            Assert.AreEqual(20, result.InterestedStatements.Count);
        }

        [Test]
        public void FilterVariableGettersByVariableVisibilityThrowsError()
        {
            var transformer = new Processors.VariableGettersFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterVariableGettersByVariableVisibility(ParsedContract, null));
        }
    }
}