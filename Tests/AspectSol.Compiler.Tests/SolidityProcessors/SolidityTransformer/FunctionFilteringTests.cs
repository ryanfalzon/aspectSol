using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Processors = AspectSol.Compiler.App.SolidityProcessors;

namespace AspectSol.Compiler.Tests.SolidityProcessors.SolidityTransformer
{
    [TestFixture]
    public class FunctionFilteringTests
    {
        private const string wildcardToken = "*";
        private const int TotalFunctions = 5;

        private readonly JToken parsedContract;

        public FunctionFilteringTests()
        {
            parsedContract = JToken.Parse(File.ReadAllText("Resources/SmartContractAST.json"));
        }

        [Test]
        public void FilterFunctionsByFunctionName()
        {
            var transformer = new Processors.SolidityTransformer();
            var result = transformer.FilterFunctionsByFunctionName(parsedContract, "store");

            Assert.AreEqual(1, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByFunctionNameWildcard()
        {
            var transformer = new Processors.SolidityTransformer();
            var result = transformer.FilterFunctionsByFunctionName(parsedContract, wildcardToken);

            Assert.AreEqual(TotalFunctions, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByFunctionNameThrowsError()
        {
            var transformer = new Processors.SolidityTransformer();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByFunctionName(parsedContract, null));
        }

        [Test]
        public void FilterFunctionsByVisibility()
        {
            var transformer = new Processors.SolidityTransformer();
            var result = transformer.FilterFunctionsByVisibility(parsedContract, "private");

            Assert.AreEqual(1, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByVisibilityWildcard()
        {
            var transformer = new Processors.SolidityTransformer();
            var result = transformer.FilterFunctionsByVisibility(parsedContract, wildcardToken);

            Assert.AreEqual(TotalFunctions, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByVisibilityThrowsError()
        {
            var transformer = new Processors.SolidityTransformer();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByVisibility(parsedContract, null));
        }

        [Test]
        public void FilterFunctionsByStateMutability()
        {
            var transformer = new Processors.SolidityTransformer();
            var result = transformer.FilterFunctionsByStateMutability(parsedContract, "pure");

            Assert.AreEqual(2, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByStateMutabilityWildcard()
        {
            var transformer = new Processors.SolidityTransformer();
            var result = transformer.FilterFunctionsByStateMutability(parsedContract, wildcardToken);

            Assert.AreEqual(TotalFunctions, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByStateMutabilityThrowsError()
        {
            var transformer = new Processors.SolidityTransformer();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByStateMutability(parsedContract, null));
        }

        [Test]
        public void FilterFunctionsByAllModifiers()
        {
            var transformer = new Processors.SolidityTransformer();

            var result1 = transformer.FilterFunctionsByAllModifiers(parsedContract, new List<string>() { "isOwner" });
            Assert.AreEqual(1, result1.InterestedFunctions.Count);

            var result2 = transformer.FilterFunctionsByAllModifiers(parsedContract, new List<string>() { "invalidModifier" });
            Assert.AreEqual(0, result2.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByAllModifiersThrowsError()
        {
            var transformer = new Processors.SolidityTransformer();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByAllModifiers(parsedContract, null));
        }

        [Test]
        public void FilterFunctionsByEitherModifiers()
        {
            var transformer = new Processors.SolidityTransformer();

            var result1 = transformer.FilterFunctionsByEitherModifiers(parsedContract, new List<string>() { "isOwner", "adminOnly" });
            Assert.AreEqual(1, result1.InterestedFunctions.Count);

            var result2 = transformer.FilterFunctionsByEitherModifiers(parsedContract, new List<string>() { "invalidModifier" });
            Assert.AreEqual(0, result2.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByEitherModifiersThrowsError()
        {
            var transformer = new Processors.SolidityTransformer();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByEitherModifiers(parsedContract, null));
        }

        [Test]
        public void FilterFunctionsByModifier()
        {
            var transformer = new Processors.SolidityTransformer();

            var result1 = transformer.FilterFunctionsByModifier(parsedContract, "isOwner", false);
            Assert.AreEqual(1, result1.InterestedFunctions.Count);

            var result2 = transformer.FilterFunctionsByModifier(parsedContract, "isOwner", true);
            Assert.AreEqual(4, result2.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByModifierThrowsError()
        {
            var transformer = new Processors.SolidityTransformer();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByModifier(parsedContract, null, false));
        }

        [Test]
        public void FilterFunctionsByParameters()
        {
            var transformer = new Processors.SolidityTransformer();

            var result1 = transformer.FilterFunctionsByParameters(parsedContract, "uint256", "num");
            Assert.AreEqual(2, result1.InterestedFunctions.Count);

            var result2 = transformer.FilterFunctionsByParameters(parsedContract, new List<(string Type, string Value)>() { ("uint256", "num") });
            Assert.AreEqual(2, result2.InterestedFunctions.Count);

            var result3 = transformer.FilterFunctionsByParameters(parsedContract, new List<(string Type, string Value)>() { ("uint128", "anotherNum") });
            Assert.AreEqual(0, result3.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByParametersThrowsError()
        {
            var transformer = new Processors.SolidityTransformer();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByParameters(parsedContract, null, "num"));
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByParameters(parsedContract, "uint256", null));
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByParameters(parsedContract, null));
        }

        [Test]
        public void FilterFunctionsByReturnParameters()
        {
            var transformer = new Processors.SolidityTransformer();

            var result1 = transformer.FilterFunctionsByReturnParameters(parsedContract, "address");
            Assert.AreEqual(1, result1.InterestedFunctions.Count);

            var result2 = transformer.FilterFunctionsByReturnParameters(parsedContract, new List<string>() { "bool" });
            Assert.AreEqual(1, result2.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByReturnParametersThrowsError()
        {
            var transformer = new Processors.SolidityTransformer();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByReturnParameters(parsedContract, returnParameter: null));
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByReturnParameters(parsedContract, returnParameters: null));
        }

        [Test]
        public void FilterFunctionCallsByInstanceName()
        {
            var transformer = new Processors.SolidityTransformer();

            var result1 = transformer.FilterFunctionCallsByInstanceName(parsedContract, "election", "HealthCheck");
            Assert.AreEqual(1, result1.InterestedStatements.Count);

            var result3 = transformer.FilterFunctionCallsByInstanceName(parsedContract, "election", "SomeOtherFunction");
            Assert.AreEqual(0, result3.InterestedStatements.Count);
        }

        [Test]
        public void FilterFunctionCallsByInstanceNameThrowsError()
        {
            var transformer = new Processors.SolidityTransformer();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionCallsByInstanceName(parsedContract, null, "HealthCheck"));
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionCallsByInstanceName(parsedContract, "election", null));
        }
    }
}