using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering.Solidity;

public class VariableGettersFiltering
{
    private const string Wildcard = "*";
    private const string ContractDefinition = "ContractDefinition";
    private const string FunctionDefinition = "FunctionDefinition";
    private const string ExpressionStatement = "ExpressionStatement";
    private const string ReturnStatement = "ReturnStatement";
    private const string VariableDeclarationStatement = "VariableDeclarationStatement";
    private const string FunctionCall = "FunctionCall";
    private const string BinaryOperation = "BinaryOperation";
    private const string IfStatement = "IfStatement";
    private const string TupleExpression = "TupleExpression";
    private const string IndexAccess = "IndexAccess";
    private const string Identifier = "Identifier";
    private const string MemberAccess = "MemberAccess";

    private readonly Dictionary<string, string> locals;

    public VariableGettersFiltering()
    {
        locals = new Dictionary<string, string>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="variableName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterVariableGettersByVariableName(JToken jToken, string variableName)
    {
        if (string.IsNullOrWhiteSpace(variableName))
            throw new ArgumentNullException(nameof(variableName));

        var interestedStatements = new Dictionary<(int, int, int), string>();

        var children = jToken["children"] as JArray;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition))
            {
                var subNodes = child["subNodes"].ToSafeList();

                var subNodePosition = 0;
                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse())
                    {
                        var statements = subNode["body"]["statements"].ToSafeList();

                        var statementPosition = 0;
                        foreach (var statement in statements)
                        {
                            if (statement["type"].Matches(ReturnStatement) && (variableName.Equals(Wildcard) || statement["expression"]["name"].Matches(variableName)))
                            {
                                interestedStatements.Add((subNodePosition, statementPosition, -1), subNode["name"].Value<string>());
                            }
                            else if (statement["type"].Matches(ReturnStatement) && statement["expression"]["type"].Matches(TupleExpression))
                            {
                                var components = statement["expression"]["components"].ToSafeList();

                                var componentPosition = 0;
                                foreach (var component in components)
                                {
                                    if (variableName.Equals(Wildcard) || component["name"].Matches(variableName))
                                    {
                                        interestedStatements.Add((subNodePosition, statementPosition, componentPosition), subNode["name"].Value<string>());
                                    }

                                    componentPosition++;
                                }
                            }
                            else if (statement["type"].Matches(VariableDeclarationStatement) && statement["initialValue"]["type"].Matches(BinaryOperation)
                                     && (variableName.Equals(Wildcard) || statement["initialValue"]["left"]["name"].Matches(variableName)
                                                                       || statement["initialValue"]["right"]["name"].Matches(variableName)))
                            {
                                interestedStatements.Add((subNodePosition, statementPosition, -1), subNode["name"].Value<string>());
                            }
                            else if (statement["type"].Matches(VariableDeclarationStatement) && (variableName.Equals(Wildcard)
                                         || statement["initialValue"]["name"].Matches(variableName)))
                            {
                                interestedStatements.Add((subNodePosition, statementPosition, -1), subNode["name"].Value<string>());
                            }
                            else if (statement["type"].Matches(IfStatement) && statement["condition"]["type"].Matches(BinaryOperation)
                                                                            && (variableName.Equals(Wildcard) || statement["condition"]["left"]["name"].Matches(variableName)
                                                                                || statement["condition"]["right"]["name"].Matches(variableName)))
                            {
                                interestedStatements.Add((subNodePosition, statementPosition, -1), subNode["name"].Value<string>());
                            }
                            else if (statement["type"].Matches(ExpressionStatement) && statement["expression"]["type"].Matches(FunctionCall))
                            {
                                var arguments = statement["expression"]["arguments"].ToSafeList();

                                var argumentPosition = 0;
                                foreach (var argument in arguments)
                                {
                                    if (argument["type"].Matches(BinaryOperation) && (variableName.Equals(Wildcard) || statement["initialValue"]["left"]["name"].Matches(variableName)
                                            || statement["initialValue"]["right"]["name"].Matches(variableName)))
                                    {
                                        interestedStatements.Add((subNodePosition, statementPosition, argumentPosition), subNode["name"].Value<string>());
                                    }

                                    argumentPosition++;
                                }
                            }

                            statementPosition++;
                        }
                    }

                    subNodePosition++;
                }
            }
        }

        return new SelectionResult
        {
            InterestedStatements = interestedStatements
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="variableType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterVariableGettersByVariableType(JToken jToken, string variableType)
    {
        if (string.IsNullOrWhiteSpace(variableType))
            throw new ArgumentNullException(nameof(variableType));

        var interestedStatements = new Dictionary<(int, int, int), string>();

        var children = jToken["children"] as JArray;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition))
            {
                var subNodes = child["subNodes"].ToSafeList();

                var subNodePosition = 0;
                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse())
                    {
                        var statements = subNode["body"]["statements"].ToSafeList();

                        var statementPosition = 0;
                        foreach (var statement in statements)
                        {
                            if (statement["type"].Matches(ReturnStatement) && (variableType.Equals(Wildcard) || IsVariableTypeMatch(statement["expression"]["name"], variableType)))
                            {
                                interestedStatements.Add((subNodePosition, statementPosition, -1), subNode["name"].Value<string>());
                            }
                            else if (statement["type"].Matches(ReturnStatement) && statement["expression"]["type"].Matches(TupleExpression))
                            {
                                var components = statement["expression"]["components"].ToSafeList();

                                var componentPosition = 0;
                                foreach (var component in components)
                                {
                                    if (variableType.Equals(Wildcard) || IsVariableTypeMatch(component["name"], variableType))
                                    {
                                        interestedStatements.Add((subNodePosition, statementPosition, componentPosition), subNode["name"].Value<string>());
                                    }

                                    componentPosition++;
                                }
                            }
                            else if (statement["type"].Matches(VariableDeclarationStatement) && statement["initialValue"]["type"].Matches(BinaryOperation)
                                     && (variableType.Equals(Wildcard) || IsVariableTypeMatch(statement["initialValue"]["left"]["name"], variableType)
                                                                       || IsVariableTypeMatch(statement["initialValue"]["right"]["name"], variableType)))
                            {
                                interestedStatements.Add((subNodePosition, statementPosition, -1), subNode["name"].Value<string>());
                            }
                            else if (statement["type"].Matches(VariableDeclarationStatement) && (variableType.Equals(Wildcard)
                                         || IsVariableTypeMatch(statement["initialValue"]["name"], variableType)))
                            {
                                interestedStatements.Add((subNodePosition, statementPosition, -1), subNode["name"].Value<string>());
                            }
                            else if (statement["type"].Matches(IfStatement) && statement["condition"]["type"].Matches(BinaryOperation)
                                                                            && (variableType.Equals(Wildcard) || IsVariableTypeMatch(statement["condition"]["left"]["name"], variableType)
                                                                                || IsVariableTypeMatch(statement["condition"]["right"]["name"], variableType)))
                            {
                                interestedStatements.Add((subNodePosition, statementPosition, -1), subNode["name"].Value<string>());
                            }
                            else if (statement["type"].Matches(ExpressionStatement) && statement["expression"]["type"].Matches(FunctionCall))
                            {
                                var arguments = statement["expression"]["arguments"].ToSafeList();

                                var argumentPosition = 0;
                                foreach (var argument in arguments)
                                {
                                    if (argument["type"].Matches(BinaryOperation) && (variableType.Equals(Wildcard)
                                            || IsVariableTypeMatch(statement["initialValue"]["left"]["name"], variableType)
                                            || IsVariableTypeMatch(statement["initialValue"]["right"]["name"], variableType)))
                                    {
                                        interestedStatements.Add((subNodePosition, statementPosition, argumentPosition), subNode["name"].Value<string>());
                                    }

                                    argumentPosition++;
                                }
                            }

                            statementPosition++;
                        }
                    }

                    subNodePosition++;
                }
            }
        }

        return new SelectionResult
        {
            InterestedStatements = interestedStatements
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="variableVisibility"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterVariableGettersByVisibility(JToken jToken, string variableVisibility)
    {
        if (string.IsNullOrWhiteSpace(variableVisibility))
            throw new ArgumentNullException(nameof(variableVisibility));

        var interestedStatements = new Dictionary<(int, int, int), string>();

        var children = jToken["children"] as JArray;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition))
            {
                var subNodes = child["subNodes"].ToSafeList();

                var subNodePosition = 0;
                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse())
                    {
                        var statements = subNode["body"]["statements"].ToSafeList();

                        var statementPosition = 0;
                        foreach (var statement in statements)
                        {
                            if (statement["type"].Matches(ReturnStatement) && (variableVisibility.Equals(Wildcard)
                                                                               || IsVariableVisibilityMatch(statement["expression"]["name"], variableVisibility)))
                            {
                                interestedStatements.Add((subNodePosition, statementPosition, -1), subNode["name"].Value<string>());
                            }
                            else if (statement["type"].Matches(ReturnStatement) && statement["expression"]["type"].Matches(TupleExpression))
                            {
                                var components = statement["expression"]["components"].ToSafeList();

                                var componentPosition = 0;
                                foreach (var component in components)
                                {
                                    if (variableVisibility.Equals(Wildcard) || IsVariableVisibilityMatch(component["name"], variableVisibility))
                                    {
                                        interestedStatements.Add((subNodePosition, statementPosition, componentPosition), subNode["name"].Value<string>());
                                    }

                                    componentPosition++;
                                }
                            }
                            else if (statement["type"].Matches(VariableDeclarationStatement) && statement["initialValue"]["type"].Matches(BinaryOperation)
                                     && (variableVisibility.Equals(Wildcard) || IsVariableVisibilityMatch(statement["initialValue"]["left"]["name"], variableVisibility)
                                                                             || IsVariableVisibilityMatch(statement["initialValue"]["right"]["name"], variableVisibility)))
                            {
                                interestedStatements.Add((subNodePosition, statementPosition, -1), subNode["name"].Value<string>());
                            }
                            else if (statement["type"].Matches(VariableDeclarationStatement) && (variableVisibility.Equals(Wildcard)
                                         || IsVariableVisibilityMatch(statement["initialValue"]["name"], variableVisibility)))
                            {
                                interestedStatements.Add((subNodePosition, statementPosition, -1), subNode["name"].Value<string>());
                            }
                            else if (statement["type"].Matches(IfStatement) && statement["condition"]["type"].Matches(BinaryOperation)
                                                                            && (variableVisibility.Equals(Wildcard) || IsVariableVisibilityMatch(statement["condition"]["left"]["name"], variableVisibility)
                                                                                || IsVariableVisibilityMatch(statement["condition"]["right"]["name"], variableVisibility)))
                            {
                                interestedStatements.Add((subNodePosition, statementPosition, -1), subNode["name"].Value<string>());
                            }
                            else if (statement["type"].Matches(ExpressionStatement) && statement["expression"]["type"].Matches(FunctionCall))
                            {
                                var arguments = statement["expression"]["arguments"].ToSafeList();

                                var argumentPosition = 0;
                                foreach (var argument in arguments)
                                {
                                    if (argument["type"].Matches(BinaryOperation) && (variableVisibility.Equals(Wildcard)
                                            || IsVariableVisibilityMatch(statement["initialValue"]["left"]["name"], variableVisibility)
                                            || IsVariableVisibilityMatch(statement["initialValue"]["right"]["name"], variableVisibility)))
                                    {
                                        interestedStatements.Add((subNodePosition, statementPosition, argumentPosition), subNode["name"].Value<string>());
                                    }

                                    argumentPosition++;
                                }
                            }

                            statementPosition++;
                        }
                    }

                    subNodePosition++;
                }
            }
        }

        return new SelectionResult
        {
            InterestedStatements = interestedStatements
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="accessKey"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    //public SelectionResult FilterVariableGettersByAccessKey(JToken jToken, string accessKey)
    //{
    //    if (string.IsNullOrWhiteSpace(accessKey))
    //        throw new ArgumentNullException(nameof(accessKey));

    //    var interestedStatements = new Dictionary<(int, int, int), string>();

    //    var children = jToken["children"] as JArray;
    //    foreach (var child in children.Children())
    //    {
    //        if (child["type"].Matches(ContractDefinition))
    //        {
    //            var subNodes = child["subNodes"].ToSafeList();

    //            var subNodePosition = 0;
    //            foreach (var subNode in subNodes)
    //            {
    //                if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse())
    //                {
    //                    var statements = subNode["body"]["statements"].ToSafeList();

    //                    var statementPosition = 0;
    //                    foreach (var statement in statements)
    //                    {
    //                        if (statement["type"].Matches(ReturnStatement) && statement["expression"]["type"].Matches(IndexAccess) && (accessKey.Equals(Wildcard)
    //                                || (statement["expression"]["index"]["type"].Matches(Identifier) && statement["expression"]["index"]["name"].Matches(accessKey))
    //                                || (statement["expression"]["index"]["type"].Matches(MemberAccess) && statement["expression"]["index"]["expression"]["name"].Matches(accessKey))))
    //                        {
    //                            interestedStatements.Add((subNodePosition, statementPosition, -1), subNode["name"].Value<string>());
    //                        }
    //                        else if (statement["type"].Matches(ReturnStatement) && statement["expression"]["type"].Matches(TupleExpression))
    //                        {
    //                            var components = statement["expression"]["components"].ToSafeList();

    //                            var componentPosition = 0;
    //                            foreach (var component in components)
    //                            {
    //                                if ((statement["expression"]["index"]["type"].Matches(Identifier) && statement["expression"]["index"]["name"].Matches(accessKey))
    //                                    || (statement["expression"]["index"]["type"].Matches(MemberAccess) && statement["expression"]["index"]["expression"]["name"].Matches(accessKey))
    //                                    || accessKey.Equals(Wildcard))
    //                                {
    //                                    interestedStatements.Add((subNodePosition, statementPosition, componentPosition), subNode["name"].Value<string>());
    //                                }

    //                                componentPosition++;
    //                            }
    //                        }
    //                        else if (statement["type"].Matches(VariableDeclarationStatement) && statement["initialValue"]["type"].Matches(BinaryOperation)
    //                                 && ((statement["initialValue"]["left"]["type"].Matches(IndexAccess) && (accessKey.Equals(Wildcard)
    //                                         || (statement["initialValue"]["left"]["index"].Matches(Identifier) && statement["initialValue"]["left"]["index"]["name"].Matches(accessKey))
    //                                         || (statement["initialValue"]["left"]["index"].Matches(MemberAccess) && statement["initialValue"]["left"]["index"]["expression"]["name"].Matches(accessKey))))
    //                                     || (statement["initialValue"]["right"]["type"].Matches(IndexAccess) && (accessKey.Equals(Wildcard)
    //                                         || (statement["initialValue"]["right"]["index"].Matches(Identifier) && statement["initialValue"]["right"]["index"]["name"].Matches(accessKey))
    //                                         || (statement["initialValue"]["right"]["index"].Matches(MemberAccess) && statement["initialValue"]["right"]["index"]["expression"]["name"].Matches(accessKey))))))
    //                        {
    //                            interestedStatements.Add((subNodePosition, statementPosition, -1), subNode["name"].Value<string>());
    //                        }
    //                        else if (statement["type"].Matches(VariableDeclarationStatement) && (variableName.Equals(Wildcard)
    //                                     || statement["initialValue"]["name"].Matches(variableName)))
    //                        {
    //                            interestedStatements.Add((subNodePosition, statementPosition, -1), subNode["name"].Value<string>());
    //                        }
    //                        else if (statement["type"].Matches(IfStatement) && statement["condition"]["type"].Matches(BinaryOperation)
    //                                                                        && (variableName.Equals(Wildcard) || statement["condition"]["left"]["name"].Matches(variableName)
    //                                                                            || statement["condition"]["right"]["name"].Matches(variableName)))
    //                        {
    //                            interestedStatements.Add((subNodePosition, statementPosition, -1), subNode["name"].Value<string>());
    //                        }
    //                        else if (statement["type"].Matches(ExpressionStatement) && statement["expression"]["type"].Matches(FunctionCall))
    //                        {
    //                            var arguments = statement["expression"]["arguments"].ToSafeList();

    //                            var argumentPosition = 0;
    //                            foreach (var argument in arguments)
    //                            {
    //                                if (argument["type"].Matches(BinaryOperation) && (variableName.Equals(Wildcard) || statement["initialValue"]["left"]["name"].Matches(variableName)
    //                                        || statement["initialValue"]["right"]["name"].Matches(variableName)))
    //                                {
    //                                    interestedStatements.Add((subNodePosition, statementPosition, argumentPosition), subNode["name"].Value<string>());
    //                                }

    //                                argumentPosition++;
    //                            }
    //                        }

    //                        statementPosition++;
    //                    }
    //                }

    //                subNodePosition++;
    //            }
    //        }
    //    }

    //    return new SelectionResult
    //    {
    //        InterestedStatements = interestedStatements
    //    };
    //}

    #region Helpers

    private bool IsVariableTypeMatch(string variableName, string variableType)
    {
        return locals.ContainsKey(variableName) && locals[variableName] == variableType;
    }

    private bool IsVariableTypeMatch(JToken variableName, string variableType)
    {
        return variableName?.Value<string>() != null && IsVariableTypeMatch(variableName.Value<string>(), variableType);
    }

    private bool IsVariableLocationMatch(string variableName, string variableLocation)
    {
        return locals.ContainsKey(variableName) && locals[variableName] == variableLocation;
    }

    private bool IsVariableLocationMatch(JToken variableName, string variableLocation)
    {
        return variableName?.Value<string>() != null && IsVariableLocationMatch(variableName.Value<string>(), variableLocation);
    }

    private bool IsVariableVisibilityMatch(string variableName, string variableVisibility)
    {
        return locals.ContainsKey(variableName) && locals[variableName] == variableVisibility;
    }

    private bool IsVariableVisibilityMatch(JToken variableName, string variableVisibility)
    {
        return variableName?.Value<string>() != null && IsVariableVisibilityMatch(variableName.Value<string>(), variableVisibility);
    }

    #endregion
}