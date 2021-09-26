using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.IO;
using Processors = AspectSol.Compiler.App.SolidityProcessors;

namespace AspectSol.Compiler.Tests.SolidityProcessors.SolidityTransformer
{
    public class VariableDefinitionFilteringTests
    {
        private const string wildcardToken = "*";

        private readonly JToken parsedContract;

        public VariableDefinitionFilteringTests()
        {
            parsedContract = JToken.Parse(File.ReadAllText("Resources/SmartContractAST.json"));
        }

        [Test]
        public void FilterVariableDefinitionByVariableType()
        {
            var transformer = new Processors.SolidityTransformer();
            var result = transformer.FilterVariableDefinitionByVariableType(parsedContract, "address");

            Assert.AreEqual(1, result.InterestedDefinitions.Count);
        }

        [Test]
        public void FilterVariableDefinitionByVariableTypeWildcard()
        {
            var transformer = new Processors.SolidityTransformer();
            var result = transformer.FilterVariableDefinitionByVariableType(parsedContract, wildcardToken);

            Assert.AreEqual(3, result.InterestedDefinitions.Count);
        }

        [Test]
        public void FilterVariableDefinitionByVariableTypeThrowsError()
        {
            var transformer = new Processors.SolidityTransformer();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterVariableDefinitionByVariableType(parsedContract, null));
        }

        [Test]
        public void FilterVariableDefinitionByVariableName()
        {
            var transformer = new Processors.SolidityTransformer();

            var result1 = transformer.FilterVariableDefinitionByVariableName(parsedContract, "owner");
            Assert.AreEqual(1, result1.InterestedDefinitions.Count);

            var result2 = transformer.FilterVariableDefinitionByVariableName(parsedContract, "someOtherOwner");
            Assert.AreEqual(0, result2.InterestedDefinitions.Count);
        }

        [Test]
        public void FilterVariableDefinitionByVariableNameWildcard()
        {
            var transformer = new Processors.SolidityTransformer();
            var result = transformer.FilterVariableDefinitionByVariableName(parsedContract, wildcardToken);

            Assert.AreEqual(3, result.InterestedDefinitions.Count);
        }

        [Test]
        public void FilterVariableDefinitionByVariableNameThrowsError()
        {
            var transformer = new Processors.SolidityTransformer();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterVariableDefinitionByVariableName(parsedContract, null));
        }
    }
}