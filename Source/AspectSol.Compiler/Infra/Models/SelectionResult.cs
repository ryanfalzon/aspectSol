using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AspectSol.Compiler.Infra.Models
{
    public class SelectionResult
    {
        public JContainer Container { get; set; }

        // Refactored code properties - Still to be tested
        public JContainer OriginalContainer { get; set; }
        public JContainer FilteredContainer { get; set; }

        /// <summary>
        /// A list of interested contracts aaspects need to be applied to
        /// </summary>
        public List<string> InterestedContracts { get; set; }

        /// <summary>
        /// Key referes to function name and value is the contract name of where the function resides
        /// </summary>
        public Dictionary<string, string> InterestedFunctions { get; set; }

        /// <summary>
        /// Key referes to definition name and value is the contract name of where the definition resides
        /// </summary>
        public Dictionary<string, string> InterestedDefinitions { get; set; }

        /// <summary>
        /// Key refers to statement index within a function and value is the function name of where the statement resides
        /// </summary>
        public Dictionary<int, string> InterestedStatements { get; set; }
    }
}