using NUnit.Framework;
using System;
using Processors = AspectSol.Lib.Domain.Filtering.Solidity;

namespace AspectSol.Lib.Tests.Domain.Filtering.Solidity
{
    public class VariableDefinitionFilteringTests : SolidityFilteringTests
    {
        private const int TotalVariableDefinitions = 4;

        [Test]
        [TestCase("address", 1)]
        public void FilterVariableDefinitionByVariableType(string variableType, int expectedCount)
        {
            var transformer = new Processors.VariableDefinitionFiltering();
            var result = transformer.FilterVariableDefinitionByVariableType(ParsedContract, variableType);

            Assert.AreEqual(expectedCount, result.InterestedDefinitions.Count);
        }

        [Test]
        public void FilterVariableDefinitionByVariableTypeWildcard()
        {
            var transformer = new Processors.VariableDefinitionFiltering();
            var result = transformer.FilterVariableDefinitionByVariableType(ParsedContract, WildcardToken);

            Assert.AreEqual(TotalVariableDefinitions, result.InterestedDefinitions.Count);
        }

        [Test]
        public void FilterVariableDefinitionByVariableTypeThrowsError()
        {
            var transformer = new Processors.VariableDefinitionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterVariableDefinitionByVariableType(ParsedContract, null));
        }

        [Test]
        [TestCase("someAddress", 1)]
        [TestCase("someOtherAddress", 0)]
        public void FilterVariableDefinitionByVariableName(string variableName, int expectedCount)
        {
            var transformer = new Processors.VariableDefinitionFiltering();

            var result = transformer.FilterVariableDefinitionByVariableName(ParsedContract, variableName);
            Assert.AreEqual(expectedCount, result.InterestedDefinitions.Count);
        }

        [Test]
        public void FilterVariableDefinitionByVariableNameWildcard()
        {
            var transformer = new Processors.VariableDefinitionFiltering();
            var result = transformer.FilterVariableDefinitionByVariableName(ParsedContract, WildcardToken);

            Assert.AreEqual(TotalVariableDefinitions, result.InterestedDefinitions.Count);
        }

        [Test]
        public void FilterVariableDefinitionByVariableNameThrowsError()
        {
            var transformer = new Processors.VariableDefinitionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterVariableDefinitionByVariableName(ParsedContract, null));
        }
    }
}