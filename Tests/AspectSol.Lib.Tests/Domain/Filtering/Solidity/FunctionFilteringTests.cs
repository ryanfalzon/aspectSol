using AspectSol.Lib.Domain.JavascriptExecution;
using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Processors = AspectSol.Lib.Domain.Filtering.Solidity;

namespace AspectSol.Lib.Tests.Domain.Filtering.Solidity
{
    [TestFixture]
    public class FunctionFilteringTests
    {
        private const string WildcardToken = "*";
        private const int TotalFunctions = 7;
        private const int TotalPrivateFunctions = 3;
        private const int TotalPublicFunctions = 4;

        private readonly JToken _parsedContract;

        public FunctionFilteringTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddNodeJS();
            serviceCollection.Configure<NodeJSProcessOptions>(options => options.ProjectPath = AppDomain.CurrentDomain.BaseDirectory);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var nodeJsService = serviceProvider.GetRequiredService<INodeJSService>();
            var scriptFactory = new ScriptFactory();

            var javascriptExecutor = new JavascriptExecutor(nodeJsService, scriptFactory);
            var smartContract = File.ReadAllTextAsync("Resources/SampleSmartContract.sol").Result;
            var result = javascriptExecutor.Execute("GenerateAST", new object[] { smartContract }).Result;

            _parsedContract = JToken.Parse(result);
        }

        [Test]
        [TestCase("store")]
        public void FilterFunctionsByFunctionName(string functionName)
        {
            var transformer = new Processors.FunctionFiltering();
            var result = transformer.FilterFunctionsByFunctionName(_parsedContract, functionName);

            Assert.AreEqual(1, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByFunctionNameWildcard()
        {
            var transformer = new Processors.FunctionFiltering();
            var result = transformer.FilterFunctionsByFunctionName(_parsedContract, WildcardToken);

            Assert.AreEqual(TotalFunctions, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByFunctionNameThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByFunctionName(_parsedContract, null));
        }

        [Test]
        public void FilterFunctionsByVisibility()
        {
            var transformer = new Processors.FunctionFiltering();

            var result1 = transformer.FilterFunctionsByVisibility(_parsedContract, "private");
            Assert.AreEqual(TotalPrivateFunctions, result1.InterestedFunctions.Count);

            var result2 = transformer.FilterFunctionsByVisibility(_parsedContract, "public");
            Assert.AreEqual(TotalPublicFunctions, result2.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByVisibilityWildcard()
        {
            var transformer = new Processors.FunctionFiltering();
            var result = transformer.FilterFunctionsByVisibility(_parsedContract, WildcardToken);

            Assert.AreEqual(TotalFunctions, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByVisibilityThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByVisibility(_parsedContract, null));
        }

        [Test]
        public void FilterFunctionsByStateMutability()
        {
            var transformer = new Processors.FunctionFiltering();
            var result = transformer.FilterFunctionsByStateMutability(_parsedContract, "pure");

            Assert.AreEqual(2, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByStateMutabilityWildcard()
        {
            var transformer = new Processors.FunctionFiltering();
            var result = transformer.FilterFunctionsByStateMutability(_parsedContract, WildcardToken);

            Assert.AreEqual(TotalFunctions, result.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByStateMutabilityThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByStateMutability(_parsedContract, null));
        }

        [Test]
        public void FilterFunctionsByAllModifiers()
        {
            var transformer = new Processors.FunctionFiltering();

            var result1 = transformer.FilterFunctionsByAllModifiers(_parsedContract, new List<string> { "onlyOwner" });
            Assert.AreEqual(1, result1.InterestedFunctions.Count);

            var result2 = transformer.FilterFunctionsByAllModifiers(_parsedContract, new List<string> { "invalidModifier" });
            Assert.AreEqual(0, result2.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByAllModifiersThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByAllModifiers(_parsedContract, null));
        }

        [Test]
        public void FilterFunctionsByEitherModifiers()
        {
            var transformer = new Processors.FunctionFiltering();

            var result1 = transformer.FilterFunctionsByEitherModifiers(_parsedContract, new List<string> { "onlyOwner", "onlyAdmins" });
            Assert.AreEqual(1, result1.InterestedFunctions.Count);

            var result2 = transformer.FilterFunctionsByEitherModifiers(_parsedContract, new List<string> { "invalidModifier" });
            Assert.AreEqual(0, result2.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByEitherModifiersThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByEitherModifiers(_parsedContract, null));
        }

        [Test]
        public void FilterFunctionsByModifier()
        {
            var transformer = new Processors.FunctionFiltering();

            var result1 = transformer.FilterFunctionsByModifier(_parsedContract, "onlyOwner", false);
            Assert.AreEqual(1, result1.InterestedFunctions.Count);

            var result2 = transformer.FilterFunctionsByModifier(_parsedContract, "onlyOwner", true);
            Assert.AreEqual(TotalFunctions - 1, result2.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByModifierThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByModifier(_parsedContract, null, false));
        }

        [Test]
        public void FilterFunctionsByParameters()
        {
            var transformer = new Processors.FunctionFiltering();

            var result1 = transformer.FilterFunctionsByParameters(_parsedContract, "uint256", "num");
            Assert.AreEqual(2, result1.InterestedFunctions.Count);

            var result2 = transformer.FilterFunctionsByParameters(_parsedContract, new List<(string Type, string Value)> { ("uint256", "num") });
            Assert.AreEqual(2, result2.InterestedFunctions.Count);

            var result3 = transformer.FilterFunctionsByParameters(_parsedContract, new List<(string Type, string Value)> { ("uint128", "anotherNum") });
            Assert.AreEqual(0, result3.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByParametersThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByParameters(_parsedContract, null, "num"));
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByParameters(_parsedContract, "uint256", null));
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByParameters(_parsedContract, null));
        }

        [Test]
        public void FilterFunctionsByReturnParameters()
        {
            var transformer = new Processors.FunctionFiltering();

            var result1 = transformer.FilterFunctionsByReturnParameters(_parsedContract, "address");
            Assert.AreEqual(0, result1.InterestedFunctions.Count);

            var result2 = transformer.FilterFunctionsByReturnParameters(_parsedContract, "uint256");
            Assert.AreEqual(4, result2.InterestedFunctions.Count);

            var result3 = transformer.FilterFunctionsByReturnParameters(_parsedContract, new List<string> { "bool" });
            Assert.AreEqual(1, result3.InterestedFunctions.Count);
        }

        [Test]
        public void FilterFunctionsByReturnParametersThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByReturnParameters(_parsedContract, returnParameter: null));
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionsByReturnParameters(_parsedContract, returnParameters: null));
        }

        [Test]
        public void FilterFunctionCallsByInstanceName()
        {
            var transformer = new Processors.FunctionFiltering();

            var result1 = transformer.FilterFunctionCallsByInstanceName(_parsedContract, "storageA", "store");
            Assert.AreEqual(1, result1.InterestedStatements.Count);

            var result2 = transformer.FilterFunctionCallsByInstanceName(_parsedContract, "storageA", "someOtherFunction");
            Assert.AreEqual(0, result2.InterestedStatements.Count);
        }

        [Test]
        public void FilterFunctionCallsByInstanceNameThrowsError()
        {
            var transformer = new Processors.FunctionFiltering();
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionCallsByInstanceName(_parsedContract, null, "HealthCheck"));
            Assert.Throws<ArgumentNullException>(() => transformer.FilterFunctionCallsByInstanceName(_parsedContract, "election", null));
        }
    }
}