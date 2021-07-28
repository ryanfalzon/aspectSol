using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspectSol.Compiler.App.SolidityTransformations
{
    public static class SolidityTransformer
    {
        private static readonly string WILDCARD = "*";
        private static readonly string CONTRACTDEFINITION = "ContractDefinition";
        private static readonly string FUNCTIONDEFINITION = "FunctionDefinition";
        private static readonly string VARIABLEDECLARATION = "VariableDeclaration";
        private static readonly string MODIFIERINVOCATION = "ModifierInvocation";
        private static readonly string INHERITANCESPECIFIER = "InheritanceSpecifier";

        #region Contract Filtering

        public static (JContainer Container, List<JToken> Contracts) GetContracts(this JContainer container, string contractName)
        {
            if (string.IsNullOrWhiteSpace(contractName))
                throw new ArgumentNullException(nameof(contractName));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Where(child => child["type"].Value<string>().Equals(CONTRACTDEFINITION) &&
                    (contractName.Equals(WILDCARD) || child["name"].Value<string>().Equals(contractName)))
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return (children, selectedChildren);
        }

        public static (JContainer Container, List<JToken> Contracts) GetContracts(this JContainer container, string contractName, string interfaceName)
        {
            if (string.IsNullOrWhiteSpace(contractName))
                throw new ArgumentNullException(nameof(contractName));
            else if(string.IsNullOrWhiteSpace(interfaceName))
                throw new ArgumentNullException(nameof(interfaceName));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Where(child => child["type"].Value<string>().Equals(CONTRACTDEFINITION) &&
                    (contractName.Equals(WILDCARD) || child["name"].Value<string>().Equals(contractName)))
                .Where(child => interfaceName.Equals("*") ||
                    (!string.IsNullOrWhiteSpace(child["baseContracts"].Value<string>()) &&
                    child["baseContracts"].Value<JArray>().ToList().Exists(baseContract => baseContract["baseName"].Value<JObject>()["namePath"].Value<string>().Equals(interfaceName))))
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return (children, selectedChildren);
        }

        public static (JContainer Container, List<JToken> Contracts) GetContracts(this JContainer container, string contractName, string functionName, string interfaceName = null)
        {
            if (string.IsNullOrWhiteSpace(contractName))
                throw new ArgumentNullException(nameof(contractName));
            else if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentNullException(nameof(functionName));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Where(child => child["type"].Value<string>().Equals(CONTRACTDEFINITION) &&
                    (contractName.Equals(WILDCARD) || child["name"].Value<string>().Equals(contractName)))
                .Where(child => interfaceName.Equals("*") ||
                    (!string.IsNullOrWhiteSpace(child["baseContracts"].Value<string>()) &&
                    child["baseContracts"].Value<JArray>().ToList().Exists(baseContract => baseContract["baseName"].Value<JObject>()["namePath"].Value<string>().Equals(interfaceName))))
                .Where(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    return subNodes.ToList().Exists(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) && 
                        (functionName.Equals(WILDCARD) || subNode["name"].Value<string>().Equals(functionName)));
                })
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return (children, selectedChildren);
        }

        public static (JContainer Container, List<JToken> Contracts) GetContracts(this JContainer container, string contractName, string functionName, string visibility, string interfaceName = null)
        {
            if (string.IsNullOrWhiteSpace(contractName))
                throw new ArgumentNullException(nameof(contractName));
            else if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentNullException(nameof(functionName));
            else if (string.IsNullOrWhiteSpace(visibility))
                throw new ArgumentNullException(nameof(visibility));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Where(child => child["type"].Value<string>().Equals(CONTRACTDEFINITION) &&
                    (contractName.Equals(WILDCARD) || child["name"].Value<string>().Equals(contractName)))
                .Where(child => interfaceName.Equals(WILDCARD) ||
                    (!string.IsNullOrWhiteSpace(child["baseContracts"].Value<string>()) &&
                    child["baseContracts"].Value<JArray>().ToList().Exists(baseContract => baseContract["baseName"].Value<JObject>()["namePath"].Value<string>().Equals(interfaceName))))
                .Where(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    return subNodes.ToList().Exists(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        (functionName.Equals(WILDCARD) || subNode["name"].Value<string>().Equals(functionName)) &&
                        (visibility.Equals(WILDCARD) || subNode["visibility"].Value<string>().Equals(visibility)));
                })
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return (children, selectedChildren);
        }

        public static (JContainer Container, List<JToken> Contracts) GetContracts(this JContainer container, string contractName, string functionName, string stateMutability, string interfaceName = null, string visibility = null)
        {
            if (string.IsNullOrWhiteSpace(contractName))
                throw new ArgumentNullException(nameof(contractName));
            else if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentNullException(nameof(functionName));
            else if (string.IsNullOrWhiteSpace(stateMutability))
                throw new ArgumentNullException(nameof(stateMutability));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Where(child => child["type"].Value<string>().Equals(CONTRACTDEFINITION) &&
                    (contractName.Equals(WILDCARD) || child["name"].Value<string>().Equals(contractName)))
                .Where(child => interfaceName.Equals(WILDCARD) ||
                    (!string.IsNullOrWhiteSpace(child["baseContracts"].Value<string>()) &&
                    child["baseContracts"].Value<JArray>().ToList().Exists(baseContract => baseContract["baseName"].Value<JObject>()["namePath"].Value<string>().Equals(interfaceName))))
                .Where(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    return subNodes.ToList().Exists(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        (functionName.Equals(WILDCARD) || subNode["name"].Value<string>().Equals(functionName)) &&
                        (string.IsNullOrWhiteSpace(visibility) || visibility.Equals(WILDCARD) || subNode["visibility"].Value<string>().Equals(visibility)) &&
                        (stateMutability.Equals(WILDCARD) || subNode["stateMutability"].Value<string>().Equals(stateMutability)));
                })
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return (children, selectedChildren);
        }

        public static (JContainer Container, List<JToken> Contracts) GetContracts(this JContainer container, string contractName, string functionName, List<string> modifiers, string interfaceName = null, string visibility = null, string stateMutability = null)
        {
            if (string.IsNullOrWhiteSpace(contractName))
                throw new ArgumentNullException(nameof(contractName));
            else if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentNullException(nameof(functionName));
            else if (modifiers.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(modifiers));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Where(child => child["type"].Value<string>().Equals(CONTRACTDEFINITION) &&
                    (contractName.Equals(WILDCARD) || child["name"].Value<string>().Equals(contractName)))
                .Where(child => interfaceName.Equals(WILDCARD) ||
                    (!string.IsNullOrWhiteSpace(child["baseContracts"].Value<string>()) &&
                    child["baseContracts"].Value<JArray>().ToList().Exists(baseContract => baseContract["baseName"].Value<JObject>()["namePath"].Value<string>().Equals(interfaceName))))
                .Where(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    return subNodes.ToList().Exists(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        (functionName.Equals(WILDCARD) || subNode["name"].Value<string>().Equals(functionName)) &&
                        (string.IsNullOrWhiteSpace(visibility) || visibility.Equals(WILDCARD) || subNode["visibility"].Value<string>().Equals(visibility)) &&
                        (string.IsNullOrWhiteSpace(stateMutability) || stateMutability.Equals(WILDCARD) || subNode["stateMutability"].Value<string>().Equals(stateMutability)) &&
                        (subNode["modifiers"].Value<JArray>().ToList()
                            .Exists(modifierInvocations => modifierInvocations["type"].Value<string>().Equals(MODIFIERINVOCATION) &&
                                modifiers.Exists(modifier => modifier.Equals(modifierInvocations["name"].Value<string>())))));
                })
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return (children, selectedChildren);
        }

        public static (JContainer Container, List<JToken> Contracts) GetContracts(this JContainer container, string contractName, string functionName, List<(string Type, string Value)> returnParameters, string interfaceName = null, string visibility = null, string stateMutability = null, List<string> modifiers = null)
        {
            if (string.IsNullOrWhiteSpace(contractName))
                throw new ArgumentNullException(nameof(contractName));
            else if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentNullException(nameof(functionName));
            else if (returnParameters.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(returnParameters));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Where(child => child["type"].Value<string>().Equals(CONTRACTDEFINITION) &&
                    (contractName.Equals(WILDCARD) || child["name"].Value<string>().Equals(contractName)))
                .Where(child => interfaceName.Equals(WILDCARD) ||
                    (!string.IsNullOrWhiteSpace(child["baseContracts"].Value<string>()) &&
                    child["baseContracts"].Value<JArray>().ToList().Exists(baseContract => baseContract["baseName"].Value<JObject>()["namePath"].Value<string>().Equals(interfaceName))))
                .Where(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    return subNodes.ToList().Exists(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        (functionName.Equals(WILDCARD) || subNode["name"].Value<string>().Equals(functionName)) &&
                        (string.IsNullOrWhiteSpace(visibility) || visibility.Equals(WILDCARD) || subNode["visibility"].Value<string>().Equals(visibility)) &&
                        (string.IsNullOrWhiteSpace(stateMutability) || stateMutability.Equals(WILDCARD) || subNode["stateMutability"].Value<string>().Equals(stateMutability)) &&
                        (modifiers != null || subNode["modifiers"].Value<JArray>().ToList()
                            .Exists(modifierInvocations => modifierInvocations["type"].Value<string>().Equals(MODIFIERINVOCATION) &&
                                modifiers.Exists(modifier => modifier.Equals(modifierInvocations["name"].Value<string>())))) &&
                        (subNode["returnParameters"].Value<JArray>().ToList()
                            .Exists(returnParameter => returnParameter["type"].Value<string>().Equals(VARIABLEDECLARATION) &&
                                returnParameters.Exists(parameter => parameter.Type.Equals(returnParameter["typeName"]["name"].Value<string>()) && parameter.Value.Equals(returnParameter["identifier"]["name"].Value<string>())))));
                })
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return (children, selectedChildren);
        }

        #endregion

        #region Function Filtering

        public static (JContainer Container, List<(JToken Parent, JToken Value)> Functions) GetFunctions(this JContainer container, string contractName, string functionName, string interfaceName = null)
        {
            if (string.IsNullOrWhiteSpace(contractName))
                throw new ArgumentNullException(nameof(contractName));
            else if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentNullException(nameof(functionName));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Where(child => child["type"].Value<string>().Equals(CONTRACTDEFINITION) &&
                    (contractName.Equals(WILDCARD) || child["name"].Value<string>().Equals(contractName)))
                .Where(child => interfaceName.Equals("*") ||
                    (!string.IsNullOrWhiteSpace(child["baseContracts"].Value<string>()) &&
                    child["baseContracts"].Value<JArray>().ToList().Exists(baseContract => baseContract["baseName"].Value<JObject>()["namePath"].Value<string>().Equals(interfaceName))))
                .Where(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    return subNodes.ToList().Exists(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        (functionName.Equals(WILDCARD) || subNode["name"].Value<string>().Equals(functionName)));
                })
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return (children, selectedChildren.SelectMany(contract =>
            {
                var subNodes = contract["subNodes"] as JArray;
                return subNodes.Select(subNode => (Parent: contract, Value: subNode)).ToList();
            }).ToList());
        }

        public static (JContainer Container, List<(JToken Parent, JToken Value)> Functions) GetFunctions(this JContainer container, string contractName, string functionName, string visibility, string interfaceName = null)
        {
            if (string.IsNullOrWhiteSpace(contractName))
                throw new ArgumentNullException(nameof(contractName));
            else if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentNullException(nameof(functionName));
            else if (string.IsNullOrWhiteSpace(visibility))
                throw new ArgumentNullException(nameof(visibility));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Where(child => child["type"].Value<string>().Equals(CONTRACTDEFINITION) &&
                    (contractName.Equals(WILDCARD) || child["name"].Value<string>().Equals(contractName)))
                .Where(child => interfaceName.Equals(WILDCARD) ||
                    (!string.IsNullOrWhiteSpace(child["baseContracts"].Value<string>()) &&
                    child["baseContracts"].Value<JArray>().ToList().Exists(baseContract => baseContract["baseName"].Value<JObject>()["namePath"].Value<string>().Equals(interfaceName))))
                .Where(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    return subNodes.ToList().Exists(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        (functionName.Equals(WILDCARD) || subNode["name"].Value<string>().Equals(functionName)) &&
                        (visibility.Equals(WILDCARD) || subNode["visibility"].Value<string>().Equals(visibility)));
                })
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return (children, selectedChildren.SelectMany(contract =>
            {
                var subNodes = contract["subNodes"] as JArray;
                return subNodes.Select(subNode => (Parent: contract, Value: subNode)).ToList();
            }).ToList());
        }

        public static (JContainer Container, List<(JToken Parent, JToken Value)> Functions) GetFunctions(this JContainer container, string contractName, string functionName, string stateMutability, string interfaceName = null, string visibility = null)
        {
            if (string.IsNullOrWhiteSpace(contractName))
                throw new ArgumentNullException(nameof(contractName));
            else if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentNullException(nameof(functionName));
            else if (string.IsNullOrWhiteSpace(stateMutability))
                throw new ArgumentNullException(nameof(stateMutability));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Where(child => child["type"].Value<string>().Equals(CONTRACTDEFINITION) &&
                    (contractName.Equals(WILDCARD) || child["name"].Value<string>().Equals(contractName)))
                .Where(child => interfaceName.Equals(WILDCARD) ||
                    (!string.IsNullOrWhiteSpace(child["baseContracts"].Value<string>()) &&
                    child["baseContracts"].Value<JArray>().ToList().Exists(baseContract => baseContract["baseName"].Value<JObject>()["namePath"].Value<string>().Equals(interfaceName))))
                .Where(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    return subNodes.ToList().Exists(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        (functionName.Equals(WILDCARD) || subNode["name"].Value<string>().Equals(functionName)) &&
                        (string.IsNullOrWhiteSpace(visibility) || visibility.Equals(WILDCARD) || subNode["visibility"].Value<string>().Equals(visibility)) &&
                        (stateMutability.Equals(WILDCARD) || subNode["stateMutability"].Value<string>().Equals(stateMutability)));
                })
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return (children, selectedChildren.SelectMany(contract =>
            {
                var subNodes = contract["subNodes"] as JArray;
                return subNodes.Select(subNode => (Parent: contract, Value: subNode)).ToList();
            }).ToList());
        }

        public static (JContainer Container, List<(JToken Parent, JToken Value)> Functions) GetFunctions(this JContainer container, string contractName, string functionName, List<string> modifiers, string interfaceName = null, string visibility = null, string stateMutability = null)
        {
            if (string.IsNullOrWhiteSpace(contractName))
                throw new ArgumentNullException(nameof(contractName));
            else if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentNullException(nameof(functionName));
            else if (modifiers.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(modifiers));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Where(child => child["type"].Value<string>().Equals(CONTRACTDEFINITION) &&
                    (contractName.Equals(WILDCARD) || child["name"].Value<string>().Equals(contractName)))
                .Where(child => interfaceName.Equals(WILDCARD) ||
                    (!string.IsNullOrWhiteSpace(child["baseContracts"].Value<string>()) &&
                    child["baseContracts"].Value<JArray>().ToList().Exists(baseContract => baseContract["baseName"].Value<JObject>()["namePath"].Value<string>().Equals(interfaceName))))
                .Where(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    return subNodes.ToList().Exists(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        (functionName.Equals(WILDCARD) || subNode["name"].Value<string>().Equals(functionName)) &&
                        (string.IsNullOrWhiteSpace(visibility) || visibility.Equals(WILDCARD) || subNode["visibility"].Value<string>().Equals(visibility)) &&
                        (string.IsNullOrWhiteSpace(stateMutability) || stateMutability.Equals(WILDCARD) || subNode["stateMutability"].Value<string>().Equals(stateMutability)) &&
                        (subNode["modifiers"].Value<JArray>().ToList()
                            .Exists(modifierInvocations => modifierInvocations["type"].Value<string>().Equals(MODIFIERINVOCATION) &&
                                modifiers.Exists(modifier => modifier.Equals(modifierInvocations["name"].Value<string>())))));
                })
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return (children, selectedChildren.SelectMany(contract =>
            {
                var subNodes = contract["subNodes"] as JArray;
                return subNodes.Select(subNode => (Parent: contract, Value: subNode)).ToList();
            }).ToList());
        }

        public static (JContainer Container, List<(JToken Parent, JToken Value)> Functions) GetFunctions(this JContainer container, string contractName, string functionName, List<(string Type, string Value)> returnParameters, string interfaceName = null, string visibility = null, string stateMutability = null, List<string> modifiers = null)
        {
            if (string.IsNullOrWhiteSpace(contractName))
                throw new ArgumentNullException(nameof(contractName));
            else if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentNullException(nameof(functionName));
            else if (returnParameters.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(returnParameters));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Where(child => child["type"].Value<string>().Equals(CONTRACTDEFINITION) &&
                    (contractName.Equals(WILDCARD) || child["name"].Value<string>().Equals(contractName)))
                .Where(child => interfaceName.Equals(WILDCARD) ||
                    (!string.IsNullOrWhiteSpace(child["baseContracts"].Value<string>()) &&
                    child["baseContracts"].Value<JArray>().ToList().Exists(baseContract => baseContract["baseName"].Value<JObject>()["namePath"].Value<string>().Equals(interfaceName))))
                .Where(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    return subNodes.ToList().Exists(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        (functionName.Equals(WILDCARD) || subNode["name"].Value<string>().Equals(functionName)) &&
                        (string.IsNullOrWhiteSpace(visibility) || visibility.Equals(WILDCARD) || subNode["visibility"].Value<string>().Equals(visibility)) &&
                        (string.IsNullOrWhiteSpace(stateMutability) || stateMutability.Equals(WILDCARD) || subNode["stateMutability"].Value<string>().Equals(stateMutability)) &&
                        (modifiers != null || subNode["modifiers"].Value<JArray>().ToList()
                            .Exists(modifierInvocations => modifierInvocations["type"].Value<string>().Equals(MODIFIERINVOCATION) &&
                                modifiers.Exists(modifier => modifier.Equals(modifierInvocations["name"].Value<string>())))) &&
                        (subNode["returnParameters"].Value<JArray>().ToList()
                            .Exists(returnParameter => returnParameter["type"].Value<string>().Equals(VARIABLEDECLARATION) &&
                                returnParameters.Exists(parameter => parameter.Type.Equals(returnParameter["typeName"]["name"].Value<string>()) && parameter.Value.Equals(returnParameter["identifier"]["name"].Value<string>())))));
                })
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return (children, selectedChildren.SelectMany(contract =>
            {
                var subNodes = contract["subNodes"] as JArray;
                return subNodes.Select(subNode => (Parent: contract, Value: subNode)).ToList();
            }).ToList());
        }

        #endregion

        #region Source Manipulation

        public static void AddContract(this JContainer container, JToken contract)
        {
            (container["children"] as JArray).Add(contract);
        }

        public static void AddContracts(this JContainer container, List<JToken> contracts)
        {
            foreach (var contract in contracts)
            {
                container.AddContract(contract);
            }
        }

        #endregion

        #region Contract Manipulation

        public static void AddContractInterface(this JToken contract, JToken @interface)
        {
            var baseContracts = contract["baseContracts"] as JArray;
            baseContracts.Add(@interface);
        }

        public static void AddContractSubNode(this JToken contract, JToken function)
        {
            (contract["subNodes"] as JArray).Add(function);
        }

        public static void AddContractSubNodes(this JToken contract, List<JToken> functions)
        {
            foreach (var function in functions)
            {
                contract.AddContractSubNode(function);
            }
        }

        public static void UpdateContractName(this JToken contract, string name)
        {

        }

        #endregion

        #region Function Manipulation

        public static void AddFunctionStatement(this JToken function, JToken statement)
        {

        }

        public static void AddFunctionStatements(this JToken function, List<JToken> statements)
        {
            foreach(var statement in statements)
            {
                function.AddFunctionStatement(statement);
            }
        }

        public static void AddFunctionModifier(this JToken function, JToken modifier)
        {

        }

        public static void AddFunctionModifiers(this JToken function, List<JToken> modifiers)
        {
            foreach (var modifier in modifiers)
            {
                function.AddFunctionModifier(modifier);
            }
        }

        public static void UpdateFunctionName(this JToken funciton, string name)
        {

        }

        public static void UpdateFuncitonVisibility(this JToken function, string visibility)
        {

        }

        public static void UpdateFunctionStateMutability(this JToken function, string stateMutability)
        {

        }

        #endregion

        #region Node Generation

        public static JToken GenerateContractInterfaceNode(string interfaceName)
        {
            return JToken.Parse(JsonConvert.SerializeObject(new
            {
                type = INHERITANCESPECIFIER,
                baseName = new
                {
                    type = "UserDefinedTypeName",
                    namePath = interfaceName
                },
                arguments = new object[] { }
            }));
        }

        public static JToken GenerateFunctionModifierNode(string modifierName)
        {
            return JToken.Parse(JsonConvert.SerializeObject(new
            {
                type = MODIFIERINVOCATION,
                name = modifierName,
                arguments = (List<object>)null
            }));
        }

        #endregion
    }
}