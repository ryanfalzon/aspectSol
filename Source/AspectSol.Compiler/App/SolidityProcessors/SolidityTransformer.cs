using AspectSol.Compiler.Domain;
using AspectSol.Compiler.Infra.Extensions;
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
        private const string WILDCARD = "*";
        private const string CONTRACTDEFINITION = "ContractDefinition";
        private const string FUNCTIONDEFINITION = "FunctionDefinition";
        private const string VARIABLEDECLARATION = "VariableDeclaration";
        private const string STATEVARIABLEDECLARATION = "StateVariableDeclaration";
        private const string MODIFIERINVOCATION = "ModifierInvocation";
        private const string INHERITANCESPECIFIER = "InheritanceSpecifier";
        private const string EXPRESSIONSTATEMENT = "ExpressionStatement";
        private const string RETURNSTATEMENT = "ReturnStatement";
        private const string VARIABLEDECLARATIONSTATEMENT = "VariableDeclarationStatement";
        private const string FUNCTIONCALL = "FunctionCall";
        private const string MEMBERACCESS = "MemberAccess";
        private const string BINARYOPERATION = "BinaryOperation";
        private const string IFSTATEMENT = "IfStatement";
        private const string ARRAYTYPENAME = "ArrayTypeName";

        public SolidityTransformer()
        {
        }

        #region Contract Filtering

        /// <summary>
        /// Filter contracts found in jToken by their name
        /// </summary>
        /// <param name="jToken"></param>
        /// <param name="contractName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public SelectionResult FilterContractsByContractName(JToken jToken, string contractName)
        {
            if (string.IsNullOrWhiteSpace(contractName))
                throw new ArgumentNullException(nameof(contractName));

            var interestedContracts = new List<string>();

            var children = jToken["children"] as JArray;
            foreach(var child in children.Children())
            {
                if(child["type"].Matches(CONTRACTDEFINITION) && child["kind"].Matches("contract") && (contractName.Equals(WILDCARD) || child["name"].Matches(contractName)))
                {
                    interestedContracts.Add(child["name"].Value<string>());
                }
            }

            return new SelectionResult
            {
                InterestedContracts = interestedContracts
            };
        }

        /// <summary>
        /// Filter contracts found in jToken by the implemented interface
        /// </summary>
        /// <param name="jToken"></param>
        /// <param name="interfaceName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public SelectionResult FilterContractsByInterfaceName(JToken jToken, string interfaceName)
        {
            if (string.IsNullOrWhiteSpace(interfaceName))
                throw new ArgumentNullException(nameof(interfaceName));

            var interestedContracts = new List<string>();

            var children = jToken["children"] as JArray;
            foreach(var child in children.Children())
            {
                if (child["type"].Matches(CONTRACTDEFINITION) && child["kind"].Matches("contract"))
                {
                    var baseContracts = child["baseContracts"].ToSafeList();

                    foreach(var baseContract in baseContracts)
                    {
                        if (interfaceName.Equals(WILDCARD) || baseContract["baseName"].Value<JObject>()["namePath"].Matches(interfaceName))
                        {
                            interestedContracts.Add(child["name"].Value<string>());
                        }
                    }
                }
            }

            return new SelectionResult
            {
                InterestedContracts = interestedContracts
            };
        }

        #endregion

        #region Function Filtering

        /// <summary>
        /// Filter functions found in jToken by their name
        /// </summary>
        /// <param name="jToken"></param>
        /// <param name="functionName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public SelectionResult FilterFunctionsByFunctionName(JToken jToken, string functionName)
        {
            if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentNullException(nameof(functionName));

            var interestedFunctions = new Dictionary<string, string>();

            var children = jToken["children"] as JArray;
            foreach (var child in children.Children())
            {
                if (child["type"].Matches(CONTRACTDEFINITION))
                {
                    var subNodes = child["subNodes"].ToSafeList();

                    foreach (var subNode in subNodes)
                    {
                        if (subNode["type"].Matches(FUNCTIONDEFINITION) && subNode["isConstructor"].IsFalse() && (functionName.Equals(WILDCARD) || subNode["name"].Matches(functionName)))
                        {
                            interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                        }
                    }
                }
            }

            return new SelectionResult
            {
                InterestedFunctions = interestedFunctions
            };
        }

        /// <summary>
        /// Filter functions found in jToken by their visibility
        /// </summary>
        /// <param name="container"></param>
        /// <param name="visibility"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public SelectionResult FilterFunctionsByVisibility(JToken jToken, string visibility)
        {
            if (string.IsNullOrWhiteSpace(visibility))
                throw new ArgumentNullException(nameof(visibility));

            var interestedFunctions = new Dictionary<string, string>();

            var children = jToken["children"] as JArray;
            foreach (var child in children.Children())
            {
                if (child["type"].Matches(CONTRACTDEFINITION))
                {
                    var subNodes = child["subNodes"].ToSafeList();

                    foreach (var subNode in subNodes)
                    {
                        if (subNode["type"].Matches(FUNCTIONDEFINITION) && subNode["isConstructor"].IsFalse() && (visibility.Equals(WILDCARD) || subNode["visibility"].Matches(visibility)))
                        {
                            interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                        }
                    }
                }
            }

            return new SelectionResult
            {
                InterestedFunctions = interestedFunctions
            };
        }

        /// <summary>
        /// Filter functions found in jToken by their state mutability
        /// </summary>
        /// <param name="jToken"></param>
        /// <param name="stateMutability"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public SelectionResult FilterFunctionsByStateMutability(JToken jToken, string stateMutability)
        {
            if (string.IsNullOrWhiteSpace(stateMutability))
                throw new ArgumentNullException(nameof(stateMutability));

            var interestedFunctions = new Dictionary<string, string>();

            var children = jToken["children"] as JArray;
            foreach (var child in children.Children())
            {
                if (child["type"].Matches(CONTRACTDEFINITION))
                {
                    var subNodes = child["subNodes"].ToSafeList();

                    foreach (var subNode in subNodes)
                    {
                        if (subNode["type"].Matches(FUNCTIONDEFINITION) && subNode["isConstructor"].IsFalse() && (stateMutability.Equals(WILDCARD) || subNode["stateMutability"].Matches(stateMutability)))
                        {
                            interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                        }
                    }
                }
            }

            return new SelectionResult
            {
                InterestedFunctions = interestedFunctions
            };
        }

        /// <summary>
        /// Filter functions found in jToken by their modifier
        /// </summary>
        /// <param name="container"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public SelectionResult FilterFunctionsByAllModifiers(JToken jToken, List<string> modifiers)
        {
            if (modifiers == null || modifiers.Count == 0)
                throw new ArgumentNullException(nameof(modifiers));

            var interestedFunctions = new Dictionary<string, string>();

            var children = jToken["children"] as JArray;
            foreach (var child in children.Children())
            {
                if (child["type"].Matches(CONTRACTDEFINITION))
                {
                    var subNodes = child["subNodes"].ToSafeList();

                    foreach (var subNode in subNodes)
                    {
                        if (subNode["type"].Matches(FUNCTIONDEFINITION) && subNode["isConstructor"].IsFalse())
                        {
                            var functionModifiers = subNode["modifiers"].ToSafeList();

                            bool allMatch = true;
                            foreach(var modifier in modifiers)
                            {
                                allMatch = functionModifiers.Exists(functionModifier => functionModifier["name"].Matches(modifier));

                                if (!allMatch)
                                {
                                    break;
                                }
                            }

                            if (allMatch)
                            {
                                interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                            }
                        }
                    }
                }
            }

            return new SelectionResult
            {
                InterestedFunctions = interestedFunctions
            };
        }

        /// <summary>
        /// Filter functions found in jToken by their modifier
        /// </summary>
        /// <param name="jToken"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public SelectionResult FilterFunctionsByEitherModifiers(JToken jToken, List<string> modifiers)
        {
            if (modifiers == null || modifiers.Count == 0)
                throw new ArgumentNullException(nameof(modifiers));

            var interestedFunctions = new Dictionary<string, string>();

            var children = jToken["children"] as JArray;
            foreach (var child in children.Children())
            {
                if (child["type"].Matches(CONTRACTDEFINITION))
                {
                    var subNodes = child["subNodes"].ToSafeList();

                    foreach (var subNode in subNodes)
                    {
                        if (subNode["type"].Matches(FUNCTIONDEFINITION) && subNode["isConstructor"].IsFalse())
                        {
                            var functionModifiers = subNode["modifiers"].ToSafeList();

                            bool foundMatch = false;
                            foreach (var modifier in modifiers)
                            {
                                foundMatch = functionModifiers.Exists(functionModifier => functionModifier["name"].Matches(modifier));

                                if (foundMatch)
                                {
                                    break;
                                }
                            }

                            if (foundMatch)
                            {
                                interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                            }
                        }
                    }
                }
            }

            return new SelectionResult
            {
                InterestedFunctions = interestedFunctions
            };
        }

        /// <summary>
        /// Filter functions found in jToken by their modifier
        /// </summary>
        /// <param name="container"></param>
        /// <param name="modifier"></param>
        /// <param name="invert"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public SelectionResult FilterFunctionsByModifier(JToken jToken, string modifier, bool invert)
        {
            if (string.IsNullOrWhiteSpace(modifier))
                throw new ArgumentNullException(nameof(modifier));

            var interestedFunctions = new Dictionary<string, string>();

            var children = jToken["children"] as JArray;
            foreach (var child in children.Children())
            {
                if (child["type"].Matches(CONTRACTDEFINITION))
                {
                    var subNodes = child["subNodes"].ToSafeList();

                    foreach (var subNode in subNodes)
                    {
                        if (subNode["type"].Matches(FUNCTIONDEFINITION) && subNode["isConstructor"].IsFalse())
                        {
                            var functionModifiers = subNode["modifiers"].ToSafeList();

                            bool foundMatch = false;
                            if (functionModifiers.Count == 0 && invert)
                            {
                                foundMatch = true;
                            }
                            else
                            {
                                foreach (var functionModifier in functionModifiers)
                                {
                                    foundMatch = invert ^ functionModifier["name"].Matches(modifier);

                                    if (foundMatch)
                                    {
                                        break;
                                    }
                                }
                            }

                            if (foundMatch)
                            {
                                interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                            }
                        }
                    }
                }
            }

            return new SelectionResult
            {
                InterestedFunctions = interestedFunctions
            };
        }

        /// <summary>
        /// Filter functions found in jToken by the parameters they accept
        /// </summary>
        /// <param name="jToken"></param>
        /// <param name="parameterType"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public SelectionResult FilterFunctionsByParameters(JToken jToken, string parameterType, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterType))
                throw new ArgumentNullException(nameof(parameterType));
            else if (string.IsNullOrWhiteSpace(parameterName))
                throw new ArgumentNullException(nameof(parameterName));

            var interestedFunctions = new Dictionary<string, string>();

            var children = jToken["children"] as JArray;
            foreach (var child in children.Children())
            {
                if (child["type"].Matches(CONTRACTDEFINITION))
                {
                    var subNodes = child["subNodes"].ToSafeList();

                    foreach (var subNode in subNodes)
                    {
                        if (subNode["type"].Matches(FUNCTIONDEFINITION) && subNode["isConstructor"].IsFalse())
                        {
                            var functionParameters = subNode["parameters"].ToSafeList();

                            bool isMatch = false;
                            foreach(var functionParameter in functionParameters)
                            {
                                if(functionParameter["type"].Matches(VARIABLEDECLARATION) && 
                                    functionParameter["typeName"]["name"].Matches(parameterType) && functionParameter["identifier"]["name"].Matches(parameterName))
                                {
                                    isMatch = true;
                                }
                            }

                            if (isMatch)
                            {
                                interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                            }
                        }
                    }
                }
            }

            return new SelectionResult
            {
                InterestedFunctions = interestedFunctions
            };
        }

        /// <summary>
        /// Filter functions found in jToken by the parameters they accept
        /// </summary>
        /// <param name="container"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public SelectionResult FilterFunctionsByParameters(JToken jToken, List<(string Type, string Value)> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                throw new ArgumentNullException(nameof(parameters));

            var interestedFunctions = new Dictionary<string, string>();

            var children = jToken["children"] as JArray;
            foreach (var child in children.Children())
            {
                if (child["type"].Matches(CONTRACTDEFINITION))
                {
                    var subNodes = child["subNodes"].ToSafeList();

                    foreach (var subNode in subNodes)
                    {
                        if (subNode["type"].Matches(FUNCTIONDEFINITION) && subNode["isConstructor"].IsFalse())
                        {
                            var functionParameters = subNode["parameters"].ToSafeList();

                            bool isMatch = functionParameters.Count > 0;
                            foreach (var functionParameter in functionParameters)
                            {
                                if (functionParameter["type"].Matches(VARIABLEDECLARATION))
                                {
                                    isMatch = parameters.Exists(parameter =>
                                        functionParameter["typeName"]["name"].Matches(parameter.Type) && functionParameter["identifier"]["name"].Matches(parameter.Value));

                                    if (!isMatch)
                                    {
                                        break;
                                    }
                                }
                            }

                            if (isMatch)
                            {
                                interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                            }
                        }
                    }
                }
            }

            return new SelectionResult
            {
                InterestedFunctions = interestedFunctions
            };
        }

        /// <summary>
        /// Filter functions found in JToken by the parameters the return
        /// </summary>
        /// <param name="jToken"></param>
        /// <param name="returnParameter"></param>
        /// <param name="returnName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public SelectionResult FilterFunctionsByReturnParameters(JToken jToken, string returnParameter)
        {
            if (string.IsNullOrWhiteSpace(returnParameter))
                throw new ArgumentNullException(nameof(returnParameter));

            var interestedFunctions = new Dictionary<string, string>();

            var children = jToken["children"] as JArray;
            foreach (var child in children.Children())
            {
                if (child["type"].Matches(CONTRACTDEFINITION))
                {
                    var subNodes = child["subNodes"].ToSafeList();

                    foreach (var subNode in subNodes)
                    {
                        if (subNode["type"].Matches(FUNCTIONDEFINITION) && subNode["isConstructor"].IsFalse())
                        {
                            var functionReturnParameters = subNode["returnParameters"].ToSafeList();

                            bool isMatch = false;
                            foreach (var functionReturnParameter in functionReturnParameters)
                            {
                                if (functionReturnParameter["type"].Matches(VARIABLEDECLARATION) && functionReturnParameter["typeName"]["name"].Matches(returnParameter))
                                {
                                    isMatch = true;
                                }
                            }

                            if (isMatch)
                            {
                                interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                            }
                        }
                    }
                }
            }

            return new SelectionResult
            {
                InterestedFunctions = interestedFunctions
            };
        }

        /// <summary>
        /// Filter functions found in JToken by the parameters the return
        /// </summary>
        /// <param name="jToken"></param>
        /// <param name="returnParameters"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public SelectionResult FilterFunctionsByReturnParameters(JToken jToken, List<string> returnParameters)
        {
            if (returnParameters == null || returnParameters.Count == 0)
                throw new ArgumentNullException(nameof(returnParameters));

            var interestedFunctions = new Dictionary<string, string>();

            var children = jToken["children"] as JArray;
            foreach (var child in children.Children())
            {
                if (child["type"].Matches(CONTRACTDEFINITION))
                {
                    var subNodes = child["subNodes"].ToSafeList();

                    foreach (var subNode in subNodes)
                    {
                        if (subNode["type"].Matches(FUNCTIONDEFINITION) && subNode["isConstructor"].IsFalse())
                        {
                            var functionReturnParameters = subNode["returnParameters"].ToSafeList();

                            var functionReturnParametersList = new List<string>();
                            foreach (var functionReturnParameter in functionReturnParameters)
                            {
                                if (functionReturnParameter["type"].Matches(VARIABLEDECLARATION))
                                {
                                    functionReturnParametersList.Add(functionReturnParameter["typeName"]["name"].Value<string>());
                                }
                            }

                            if (returnParameters.SequenceEqual(functionReturnParametersList))
                            {
                                interestedFunctions.Add(subNode["name"].Value<string>(), child["name"].Value<string>());
                            }
                        }
                    }
                }
            }

            return new SelectionResult
            {
                InterestedFunctions = interestedFunctions
            };
        }

        /// <summary>
        /// Filter function statements that call a function found in an instanced contract
        /// </summary>
        /// <param name="container"></param>
        /// <param name="instanceName"></param>
        /// <param name="functionName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public SelectionResult FilterFunctionCallsByInstanceName(JToken jToken, string instanceName, string functionName)
        {
            if (string.IsNullOrWhiteSpace(instanceName))
                throw new ArgumentNullException(nameof(instanceName));
            else if (string.IsNullOrWhiteSpace(functionName))
                throw new ArgumentNullException(nameof(functionName));

            var interestedStatements = new Dictionary<int, string>();

            var children = jToken["children"] as JArray;
            foreach (var child in children.Children())
            {
                if (child["type"].Matches(CONTRACTDEFINITION))
                {
                    var subNodes = child["subNodes"].ToSafeList();

                    foreach (var subNode in subNodes)
                    {
                        if (subNode["type"].Matches(FUNCTIONDEFINITION) && subNode["isConstructor"].IsFalse())
                        {
                            var statements = subNode["body"]["statements"].ToSafeList();

                            var statementPosition = 0;
                            foreach (var statement in statements)
                            {
                                if (statement["type"].Matches(EXPRESSIONSTATEMENT) && statement["expression"]["type"].Matches(FUNCTIONCALL)
                                    && statement["expression"]["expression"]["expression"]["name"].Matches(instanceName) 
                                    && statement["expression"]["expression"]["memberName"].Matches(functionName))
                                {
                                    interestedStatements.Add(statementPosition, subNode["name"].Value<string>());
                                }

                                statementPosition++;
                            }
                        }
                    }
                }
            }

            return new SelectionResult
            {
                InterestedStatements = interestedStatements
            };
        }

        #endregion

        #region Variable Definition Filtering

        /// <summary>
        /// Filter variable definitions found in jToken by their type
        /// </summary>
        /// <param name="container"></param>
        /// <param name="variableType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public SelectionResult FilterVariableDefinitionByVariableType(JToken jToken, string variableType)
        {
            if (string.IsNullOrWhiteSpace(variableType))
                throw new ArgumentNullException(nameof(variableType));

            var interestedDefinitions = new Dictionary<string, string>();

            var children = jToken["children"] as JArray;
            foreach (var child in children.Children())
            {
                if (child["type"].Matches(CONTRACTDEFINITION))
                {
                    var subNodes = child["subNodes"].ToSafeList();

                    foreach (var subNode in subNodes)
                    {
                        if (subNode["type"].Matches(STATEVARIABLEDECLARATION))
                        {
                            var variables = subNode["variables"].ToSafeList();
                            foreach(var variable in variables)
                            {
                                if(variable["type"].Matches(VARIABLEDECLARATION) && (variableType.Equals(WILDCARD) || variable["typeName"]["name"].Matches(variableType)))
                                {
                                    interestedDefinitions.Add(variable["name"].Value<string>(), child["name"].Value<string>());
                                }
                            }
                        }
                    }
                }
            }

            return new SelectionResult
            {
                InterestedDefinitions = interestedDefinitions
            };
        }

        /// <summary>
        /// Filter variable definitions found in jToken by their name
        /// </summary>
        /// <param name="container"></param>
        /// <param name="variableName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public SelectionResult FilterVariableDefinitionByVariableName(JToken jToken, string variableName)
        {
            if (string.IsNullOrWhiteSpace(variableName))
                throw new ArgumentNullException(nameof(variableName));

            var interestedDefinitions = new Dictionary<string, string>();

            var children = jToken["children"] as JArray;
            foreach (var child in children.Children())
            {
                if (child["type"].Matches(CONTRACTDEFINITION))
                {
                    var subNodes = child["subNodes"].ToSafeList();

                    foreach (var subNode in subNodes)
                    {
                        if (subNode["type"].Matches(STATEVARIABLEDECLARATION))
                        {
                            var variables = subNode["variables"].ToSafeList();
                            foreach (var variable in variables)
                            {
                                if (variable["type"].Matches(VARIABLEDECLARATION) && (variableName.Equals(WILDCARD) || variable["name"].Matches(variableName)))
                                {
                                    interestedDefinitions.Add(variable["name"].Value<string>(), child["name"].Value<string>());
                                }
                            }
                        }
                    }
                }
            }

            return new SelectionResult
            {
                InterestedDefinitions = interestedDefinitions
            };
        }

        #endregion

        #region Variable Getters Filtering

        public SelectionResult FilterVariableGettersByVariableType(JToken jToken, string variableName)
        {
            if (string.IsNullOrWhiteSpace(variableName))
                throw new ArgumentNullException(nameof(variableName));

            var interestedStatements = new Dictionary<int, string>();

            var children = jToken["children"] as JArray;
            foreach (var child in children.Children())
            {
                if (child["type"].Matches(CONTRACTDEFINITION))
                {
                    var subNodes = child["subNodes"].ToSafeList();

                    foreach (var subNode in subNodes)
                    {
                        if (subNode["type"].Matches(FUNCTIONDEFINITION) && subNode["isConstructor"].IsFalse())
                        {
                            var statements = subNode["body"]["statements"].ToSafeList();

                            var statementPosition = 0;
                            foreach (var statement in statements)
                            {
                                if (statement["type"].Matches(RETURNSTATEMENT) && statement["expression"]["name"].Matches(variableName))
                                {
                                    interestedStatements.Add(statementPosition, subNode["name"].Value<string>());
                                }
                                else if(statement["type"].Matches(VARIABLEDECLARATIONSTATEMENT) && statement["initialValue"]["type"].Matches(BINARYOPERATION)
                                    && (statement["initialValue"]["left"]["name"].Matches(variableName) || statement["initialValue"]["right"]["name"].Matches(variableName)))
                                {
                                    interestedStatements.Add(statementPosition, subNode["name"].Value<string>());
                                }
                                else if(statement["type"].Matches(VARIABLEDECLARATIONSTATEMENT) && statement["initialValue"]["name"].Matches(variableName))
                                {
                                    interestedStatements.Add(statementPosition, subNode["name"].Value<string>());
                                }
                                else if(statement["type"].Matches(IFSTATEMENT) && statement["condition"]["type"].Matches(BINARYOPERATION)
                                    && (statement["condition"]["left"]["name"].Matches(variableName) || statement["condition"]["right"]["name"].Matches(variableName)))
                                {
                                    interestedStatements.Add(statementPosition, subNode["name"].Value<string>());
                                }
                                else if(statement["type"].Matches(EXPRESSIONSTATEMENT) && statement["expression"]["type"].Matches(FUNCTIONCALL))
                                {
                                    var arguments = statement["expression"]["arguments"].ToSafeList();
                                    foreach(var argument in arguments)
                                    {
                                        if(argument["type"].Matches(BINARYOPERATION) && (statement["initialValue"]["left"]["name"].Matches(variableName)
                                            || statement["initialValue"]["right"]["name"].Matches(variableName)))
                                        {
                                            interestedStatements.Add(statementPosition, subNode["name"].Value<string>());
                                        }
                                    }
                                    interestedStatements.Add(statementPosition, subNode["name"].Value<string>());
                                }

                                statementPosition++;
                            }
                        }
                    }
                }
            }

            return new SelectionResult
            {
                InterestedStatements = interestedStatements
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

        /// <summary>
        /// Add new contract token to source
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newContract"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddContract(ref JToken source, JToken newContract)
        {
            if(newContract == null)
                throw new ArgumentNullException(nameof(newContract));

            (source["children"] as JArray).Add(newContract);
        }

        /// <summary>
        /// Add new contract tokens to source
        /// </summary>
        /// <param name="source"></param>
        /// <param name="newContracts"></param>
        public void AddContracts(ref JToken source, List<JToken> newContracts)
        {
            if (newContracts == null || newContracts.Count == 0)
                throw new ArgumentNullException(nameof(newContracts));

            foreach (var contract in newContracts)
            {
                AddContract(ref source, contract);
            }
        }

        #endregion

        #region Contract Manipulation

        /// <summary>
        /// Add new interface implementation to interested contracts
        /// </summary>
        /// <param name="source"></param>
        /// <param name="interestedContracts"></param>
        /// <param name="newContractInterface"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddContractInterface(ref JToken source, List<string> interestedContracts, JToken newContractInterface)
        {
            
            if (interestedContracts == null || interestedContracts.Count == 0)
                throw new ArgumentNullException(nameof(interestedContracts));
            else if (newContractInterface == null)
                throw new ArgumentNullException(nameof(newContractInterface));

            // TODO - AddContractInterface
        }

        /// <summary>
        /// Add new sub node to interested contract
        /// </summary>
        /// <param name="source"></param>
        /// <param name="interestedContracts"></param>
        /// <param name="newSubNode"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddContractSubNode(ref JToken source, List<string> interestedContracts, JToken newSubNode)
        {
            if (interestedContracts == null || interestedContracts.Count == 0)
                throw new ArgumentNullException(nameof(interestedContracts));
            else if (newSubNode == null)
                throw new ArgumentNullException(nameof(newSubNode));

            // TODO - AddContractSubNode
        }

        /// <summary>
        /// Add new sub nodes to interested contracts
        /// </summary>
        /// <param name="source"></param>
        /// <param name="interestedContracts"></param>
        /// <param name="newSubNodes"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddContractSubNodes(ref JToken source, List<string> interestedContracts, List<JToken> newSubNodes)
        {
            if (interestedContracts == null || interestedContracts.Count == 0)
                throw new ArgumentNullException(nameof(interestedContracts));
            else if (newSubNodes == null || newSubNodes.Count == 0)
                throw new ArgumentNullException(nameof(newSubNodes));

            foreach (var subNode in newSubNodes)
            {
                AddContractSubNode(ref source, interestedContracts, subNode);
            }
        }

        /// <summary>
        /// Update name of interested contracts
        /// </summary>
        /// <param name="source"></param>
        /// <param name="interestedContracts"></param>
        /// <param name="newName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void UpdateContractName(ref JToken source, List<string> interestedContracts, string newName)
        {
            if (interestedContracts == null || interestedContracts.Count == 0)
                throw new ArgumentNullException(nameof(interestedContracts));
            else if (newName == null)
                throw new ArgumentNullException(nameof(newName));

            // TODO - UpdateContractName
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

        public void AddTagToFunction(JContainer container, string functionName, string contractName, string tag)
        {
            // TODO
        }

        public void AddTagToFunctions(JContainer container, Dictionary<string, string> interestedFunctions, string tag)
        {
            foreach(var interestedFunction in interestedFunctions)
            {
                AddTagToFunction(container, interestedFunction.Key, interestedFunction.Value, tag);
            }
        }

        public void RemoveTagFromFunction(JContainer container, string functionName, string contractName, string tag)
        {
            // TODO
        }

        public void RemoveTagFromFunctions(JContainer container, Dictionary<string, string> interestedFunctions, string tag)
        {
            foreach (var interestedFunction in interestedFunctions)
            {
                RemoveTagFromFunction(container, interestedFunction.Key, interestedFunction.Value, tag);
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