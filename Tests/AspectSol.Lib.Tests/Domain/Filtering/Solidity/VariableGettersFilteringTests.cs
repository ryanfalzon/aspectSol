using NUnit.Framework;
using Processors = AspectSol.Lib.Domain.Filtering.Solidity;

namespace AspectSol.Lib.Tests.Domain.Filtering.Solidity
{
    public class VariableGettersFilteringTests : SolidityFilteringTests
    {
        [Test]
        public void FilterVariableGettersByVariableName()
        {
            var transformer = new Processors.VariableGettersFiltering();

            var result1 = transformer.FilterVariableGettersByVariableName(ParsedContract, "number");
            Assert.AreEqual(7, result1.InterestedStatements.Count);

            var result2 = transformer.FilterVariableGettersByVariableName(ParsedContract, "owner");
            Assert.AreEqual(1, result2.InterestedStatements.Count);
        }

        [Test]
        public void FilterVariableGettersByVariableNameWildcard()
        {
            var transformer = new Processors.VariableGettersFiltering();

            var result = transformer.FilterVariableGettersByVariableName(ParsedContract, WildcardToken);
            Assert.AreEqual(7, result.InterestedStatements.Count);
        }
    }
}