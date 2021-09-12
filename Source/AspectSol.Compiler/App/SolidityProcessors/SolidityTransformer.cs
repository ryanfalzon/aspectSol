using AspectSol.Compiler.Domain;
using AspectSol.Compiler.Infra.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspectSol.Compiler.App.SolidityProcessors
{
    public class SolidityTransformer : IContractTransformer
    {
        private readonly string WILDCARD = "*";
        private readonly string CONTRACTDEFINITION = "ContractDefinition";
        private readonly string FUNCTIONDEFINITION = "FunctionDefinition";
        private readonly string VARIABLEDECLARATION = "VariableDeclaration";
        private readonly string MODIFIERINVOCATION = "ModifierInvocation";
        private readonly string INHERITANCESPECIFIER = "InheritanceSpecifier";
        private readonly string EXPRESSIONSTATEMENT = "ExpressionStatement";
        private readonly string FUNCTIONCALL = "FunctionCall";
        private readonly string MEMBERACCESS = "MemberAccess";
        private readonly string BINARYOPERATION = "BinaryOperation";
        private readonly string ARRAYTYPENAME = "ArrayTypeName";

        public SolidityTransformer()
        {
        }

        #region Contract Filtering

        public SelectionResult FilterContractsByContractName(JContainer container, string contractName)
        {
            if (string.IsNullOrWhiteSpace(contractName))
                throw new ArgumentNullException(nameof(contractName));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Where(child => child["type"].Value<string>().Equals(CONTRACTDEFINITION) &&
                    (contractName.Equals(WILDCARD) || child["name"].Value<string>().Equals(contractName)))
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));
            container["children"] = new JArray(selectedChildren);

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterContractsByInterfaceName(JContainer container, string interfaceName)
        {
            if (string.IsNullOrWhiteSpace(interfaceName))
                throw new ArgumentNullException(nameof(interfaceName));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Where(child => interfaceName.Equals("*") ||
                    (!string.IsNullOrWhiteSpace(child["baseContracts"].Value<string>()) &&
                    child["baseContracts"].Value<JArray>().ToList().Exists(baseContract => baseContract["baseName"].Value<JObject>()["namePath"].Value<string>().Equals(interfaceName))))
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return new SelectionResult
            {
                Container = container
            };
        }

        #endregion

        #region Function Filtering

        public SelectionResult FilterFunctionsByFunctionName(JContainer container, string functionName)
        {
            if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentNullException(nameof(functionName));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Select(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    subNodes = new JArray(subNodes.ToList().Where(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        (functionName.Equals(WILDCARD) || subNode["name"].Value<string>().Equals(functionName))));

                    return contract;
                })
                .Where(contract => (contract["subNodes"] as JArray).Count > 0)
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterFunctionsByVisibility(JContainer container, string visibility)
        {
            if (string.IsNullOrWhiteSpace(visibility))
                throw new ArgumentNullException(nameof(visibility));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Select(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    subNodes = new JArray(subNodes.ToList().Where(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        (visibility.Equals(WILDCARD) || subNode["visibility"].Value<string>().Equals(visibility))));

                    return contract;
                })
                .Where(contract => (contract["subNodes"] as JArray).Count > 0)
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterFunctionsByStateMutability(JContainer container, string stateMutability)
        {
            if (string.IsNullOrWhiteSpace(stateMutability))
                throw new ArgumentNullException(nameof(stateMutability));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Select(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    subNodes = new JArray(subNodes.ToList().Where(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        (stateMutability.Equals(WILDCARD) || subNode["stateMutability"].Value<string>().Equals(stateMutability))));

                    return contract;
                })
                .Where(contract => (contract["subNodes"] as JArray).Count > 0)
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterFunctionsByAllModifiers(JContainer container, List<string> modifiers)
        {
            if (modifiers.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(modifiers));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Select(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    subNodes = new JArray(subNodes.ToList().Where(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        subNode["modifiers"].Value<JArray>().ToList()
                            .Where(modifierInvocations => modifierInvocations["type"].Value<string>().Equals(MODIFIERINVOCATION) &&
                                modifiers.Exists(modifier => modifier.Equals(modifierInvocations["name"].Value<string>())))
                            .Count() == 2));

                    return contract;
                })
                .Where(contract => (contract["subNodes"] as JArray).Count > 0)
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterFunctionsByEitherModifiers(JContainer container, List<string> modifiers)
        {
            if (modifiers.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(modifiers));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Select(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    subNodes = new JArray(subNodes.ToList().Where(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        subNode["modifiers"].Value<JArray>().ToList()
                            .Exists(modifierInvocations => modifierInvocations["type"].Value<string>().Equals(MODIFIERINVOCATION) &&
                                modifiers.Exists(modifier => modifier.Equals(modifierInvocations["name"].Value<string>())))));

                    return contract;
                })
                .Where(contract => (contract["subNodes"] as JArray).Count > 0)
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterFunctionsByModifier(JContainer container, string modifier, bool invert)
        {
            if (string.IsNullOrWhiteSpace(modifier))
                throw new ArgumentNullException(nameof(modifier));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Select(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    subNodes = new JArray(subNodes.ToList().Where(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        subNode["modifiers"].Value<JArray>().ToList()
                            .Exists(modifierInvocations => modifierInvocations["type"].Value<string>().Equals(MODIFIERINVOCATION) &&
                                (invert ^ modifier.Equals(modifierInvocations["name"].Value<string>())))));

                    return contract;
                })
                .Where(contract => (contract["subNodes"] as JArray).Count > 0)
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterFunctionsByParameters(JContainer container, List<(string Type, string Value)> parameters)
        {
            if (parameters.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(parameters));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Select(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    subNodes = new JArray(subNodes.ToList().Where(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        subNode["parameters"].Value<JArray>().ToList()
                            .Exists(functionParameter => functionParameter["type"].Value<string>().Equals(VARIABLEDECLARATION) &&
                                parameters.Exists(parameter => parameter.Type.Equals(functionParameter["typeName"]["name"].Value<string>()) && parameter.Value.Equals(functionParameter["identifier"]["name"].Value<string>())))));

                    return contract;
                })
                .Where(contract => (contract["subNodes"] as JArray).Count > 0)
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterFunctionsByParameter(JContainer container, string parameterType, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterType))
                throw new ArgumentNullException(nameof(parameterType));
            else if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException(nameof(parameterName));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Select(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    subNodes = new JArray(subNodes.ToList().Where(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        subNode["parameters"].Value<JArray>().ToList()
                            .Exists(functionParameter => functionParameter["type"].Value<string>().Equals(VARIABLEDECLARATION) &&
                                parameterType.Equals(functionParameter["typeName"]["name"].Value<string>()) && parameterName.Equals(functionParameter["identifier"]["name"].Value<string>()))));

                    return contract;
                })
                .Where(contract => (contract["subNodes"] as JArray).Count > 0)
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterFunctionsByReturnParameters(JContainer container, List<(string Type, string Value)> returnParameters)
        {
            if (returnParameters.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(returnParameters));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Select(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    subNodes = new JArray(subNodes.ToList().Where(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        subNode["returnParameters"].Value<JArray>().ToList()
                            .Exists(returnParameter => returnParameter["type"].Value<string>().Equals(VARIABLEDECLARATION) &&
                                returnParameters.Exists(parameter => parameter.Type.Equals(returnParameter["typeName"]["name"].Value<string>()) && parameter.Value.Equals(returnParameter["identifier"]["name"].Value<string>())))));

                    return contract;
                })
                .Where(contract => (contract["subNodes"] as JArray).Count > 0)
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterFunctionsByReturnParameters(JContainer container, string returnType, string returnName)
        {
            if (string.IsNullOrWhiteSpace(returnType))
                throw new ArgumentNullException(nameof(returnType));
            else if (string.IsNullOrWhiteSpace(returnName))
                throw new ArgumentNullException(nameof(returnName));

            var children = container["children"] as JArray;
            var selectedChildren = children.Children()
                .Select(contract =>
                {
                    var subNodes = contract["subNodes"] as JArray;
                    subNodes = new JArray(subNodes.ToList().Where(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) &&
                        subNode["returnParameters"].Value<JArray>().ToList()
                            .Exists(returnParameter => returnParameter["type"].Value<string>().Equals(VARIABLEDECLARATION) &&
                                returnParameter["typeName"]["name"].Value<string>().Equals(returnType) && returnParameter["identifier"]["name"].Value<string>().Equals(returnName))));

                    return contract;
                })
                .Where(contract => (contract["subNodes"] as JArray).Count > 0)
                .ToList();

            selectedChildren.ForEach(child => children.Remove(child));

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterFunctionCallsByInstanceName(JContainer container, string instanceName, string functionName)
        {
            if (string.IsNullOrWhiteSpace(instanceName))
                throw new ArgumentNullException(nameof(instanceName));
            else if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentNullException(nameof(functionName));

            // TODO - FilterFunctionCallsByInstanceName

            return new SelectionResult
            {
                Container = container
            };
        }

        #endregion

        #region Variable Definition Filtering

        public SelectionResult FilterVariableDefinitionByContractAddress(JContainer container, string contractAddress)
        {
            if (string.IsNullOrWhiteSpace(contractAddress))
                throw new ArgumentNullException(nameof(contractAddress));

            // TODO - FilterVariableDefinitionByContractAddress

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterVariableDefinitionByVariableType(JContainer container, string variableType)
        {
            if (string.IsNullOrWhiteSpace(variableType))
                throw new ArgumentNullException(nameof(variableType));

            // TODO - FilterVariableDefinitionByVariableName

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterVariableDefinitionByVariableName(JContainer container, string variableName)
        {
            if (string.IsNullOrWhiteSpace(variableName))
                throw new ArgumentNullException(nameof(variableName));

            // TODO - FilterVariableDefinitionByVariableName

            return new SelectionResult
            {
                Container = container
            };
        }

        #endregion

        #region Variable Getters Filtering

        public SelectionResult FilterVariableGettersByVariableType(JContainer container, string variableName)
        {
            if (string.IsNullOrWhiteSpace(variableName))
                throw new ArgumentNullException(nameof(variableName));

            // TODO - FilterVariableGettersByVariableType

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterVariableGettersByVariableLocation(JContainer container, string contractName)
        {
            if (string.IsNullOrWhiteSpace(contractName))
                throw new ArgumentNullException(nameof(contractName));

            // TOOO - FilterVariableGettersByVariableLocation

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterVariableGettersByVariableName(JContainer container, string variableType)
        {
            if (string.IsNullOrWhiteSpace(variableType))
                throw new ArgumentNullException(nameof(variableType));

            // TODO - FilterVariableGettersByVariableName

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterVariableGettersByAccessKey(JContainer container, string accessKey)
        {
            if (string.IsNullOrWhiteSpace(accessKey))
                throw new ArgumentNullException(nameof(accessKey));

            // TODO - FilterVariableGettersByAccessKey

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterVariableGettersByVisibility(JContainer container, string visibility)
        {
            if (string.IsNullOrWhiteSpace(visibility))
                throw new ArgumentNullException(nameof(visibility));

            // TODO - FilterVariableGettersByVisibility

            return new SelectionResult
            {
                Container = container
            };
        }

        #endregion

        #region Variable Setters Filtering

        public SelectionResult FilterVariableSettersByVariableType(JContainer container, string variableName)
        {
            if (string.IsNullOrWhiteSpace(variableName))
                throw new ArgumentNullException(nameof(variableName));

            // TODO - FilterVariableSettersByVariableType

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterVariableSettersByVariableLocation(JContainer container, string contractName)
        {
            if (string.IsNullOrWhiteSpace(contractName))
                throw new ArgumentNullException(nameof(contractName));

            // TOOO - FilterVariableSettersByVariableLocation

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterVariableSettersByVariableName(JContainer container, string variableType)
        {
            if (string.IsNullOrWhiteSpace(variableType))
                throw new ArgumentNullException(nameof(variableType));

            // TODO - FilterVariableSettersByVariableName

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterVariableSettersByAccessKey(JContainer container, string accessKey)
        {
            if (string.IsNullOrWhiteSpace(accessKey))
                throw new ArgumentNullException(nameof(accessKey));

            // TODO - FilterVariableSettersByAccessKey

            return new SelectionResult
            {
                Container = container
            };
        }

        public SelectionResult FilterVariableSettersByVisibility(JContainer container, string visibility)
        {
            if (string.IsNullOrWhiteSpace(visibility))
                throw new ArgumentNullException(nameof(visibility));

            // TODO - FilterVariableSettersByVisibility

            return new SelectionResult
            {
                Container = container
            };
        }

        #endregion

        #region Interface Filtering

        public SelectionResult FilterContainerByInterfaceImplementation(JContainer container, string interfaceName, bool invert)
        {
            if (string.IsNullOrWhiteSpace(interfaceName))
                throw new ArgumentNullException(nameof(interfaceName));

            var children = container["children"] as JArray;
            var @interface = children.Children()
                .Where(child => child["type"].Value<string>().Equals(CONTRACTDEFINITION) &&
                    (child["name"].Value<string>().Equals(interfaceName)))
                .FirstOrDefault();

            if (@interface != null)
            {
                var filteredContainer = FilterContractsByInterfaceName(container, interfaceName).Container;

                var selectedChildren = filteredContainer["children"] as JArray;
                var filteredChildren = selectedChildren.Children()
                    .Select(contract =>
                    {
                        var subNodes = contract["subNodes"] as JArray;
                        subNodes = new JArray(subNodes.ToList()
                            .Where(subNode => subNode["type"].Value<string>().Equals(FUNCTIONDEFINITION) && !(invert ^ DoesInterfaceContain(@interface, subNode))));

                        return contract;
                    })
                    .Where(contract => (contract["subNodes"] as JArray).Count > 0)
                    .ToList();

                filteredChildren.ForEach(child => selectedChildren.Remove(child));
                container["children"] = new JArray(filteredChildren);

                return new SelectionResult { Container = container };
            }
            else
            {
                container["children"] = new JArray();

                return new SelectionResult { Container = container };
            }
        }

        private bool DoesInterfaceContain(JToken @interface, JToken function)
        {
            // TODO - DoesInterfaceContain
            return true;
        }

        #endregion

        #region Source Manipulation

        public void AddContract(JContainer container, JToken contract)
        {
            (container["children"] as JArray).Add(contract);
        }

        public void AddContracts(JContainer container, List<JToken> contracts)
        {
            foreach (var contract in contracts)
            {
                AddContracts(container, contracts);
            }
        }

        #endregion

        #region Contract Manipulation

        public void AddContractInterface(JToken contract, JToken @interface)
        {
            var baseContracts = contract["baseContracts"] as JArray;
            baseContracts.Add(@interface);
        }

        public void AddContractSubNode(JToken contract, JToken subNode)
        {
            (contract["subNodes"] as JArray).Add(subNode);
        }

        public void AddContractSubNodes(JToken contract, List<JToken> subNodes)
        {
            foreach (var subNode in subNodes)
            {
                AddContractSubNode(contract, subNode);
            }
        }

        public void UpdateContractName(JToken contract, string name)
        {
            contract["name"] = name;
        }

        #endregion

        #region Function Manipulation

        public void AddFunctionStatement(JToken function, JToken statement, JToken before = null, JToken after = null)
        {
            // TODO - Add before and after statements
            var functionStatements = function["body"]["statements"] as JArray;
            functionStatements.Add(statement);
        }

        public void AddFunctionStatements(JToken function, List<JToken> statements, JToken before = null, JToken after = null)
        {
            foreach (var statement in statements)
            {
                AddFunctionStatement(function, statement, before, after);
            }
        }

        public void AddFunctionModifier(JToken function, JToken modifier)
        {
            var modifiers = function["modifiers"] as JArray;
            modifiers.Add(modifier);
        }

        public void AddFunctionModifiers(JToken function, List<JToken> modifiers)
        {
            foreach (var modifier in modifiers)
            {
                AddFunctionModifier(function, modifier);
            }
        }

        public void UpdateFunctionName(JToken function, string name)
        {
            function["name"] = name;
        }

        public void UpdateFuncitonVisibility(JToken function, string visibility)
        {
            function["visibility"] = visibility;
        }

        public void UpdateFunctionStateMutability(JToken function, string stateMutability)
        {
            function["stateMutability"] = stateMutability;
        }

        #endregion

        #region Variable Manipulation

        public void UpdateVariableVisibility(JToken variable, string visibility)
        {
            variable["visibility"] = visibility;
        }

        #endregion

        #region Node Generation

        public JToken GenerateContractInterfaceNode(string interfaceName)
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

        public JToken GenerateFunctionModifierNode(string modifierName)
        {
            return JToken.Parse(JsonConvert.SerializeObject(new
            {
                type = MODIFIERINVOCATION,
                name = modifierName,
                arguments = (List<object>)null
            }));
        }

        public JToken GenerateVariableNode(string variableType, string variableName, string variableVisibility)
        {
            // TODO - Complete variable node generation
            return JToken.Parse(JsonConvert.SerializeObject(new
            {
                type = VARIABLEDECLARATION,
                typeName = new
                {
                    type = "",
                    baseTypeName = new
                    {
                        type = "",
                        namePath = ""
                    },
                    length = (object)null
                },
                name = variableName,
                identifier = new
                {
                    type = "Identifier",
                    name = variableName
                },
                expression = (object)null,
                visibility = variableVisibility,
                isStateVar = true,
                isDeclaredConst = false,
                isIndexed = false,
                isImmutable = false,
                @override = (object)null,
                storageLocation = (object)null
            }));
        }

        public JToken GenerateModifierNode(string modifierName)
        {
            // TODO - GenerateModifierNode
            throw new NotImplementedException();
        }

        public JToken GenerateFunctionStatement(string statement)
        {
            // TODO - GenerateFunctionStatement
            throw new NotImplementedException();
        }

        public List<JToken> GenerateFunctionStatements(List<string> statements)
        {
            // TODO - GenerateFunctionStatements
            throw new NotImplementedException();
        }

        #endregion
    }
}