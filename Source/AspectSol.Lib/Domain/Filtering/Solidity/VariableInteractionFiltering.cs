using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering.Solidity;

public abstract class VariableFiltering : IVariableInteractionFiltering
{
    protected const string ContractDefinition = "ContractDefinition";
    protected const string Wildcard = "*";
    protected const string FunctionDefinition = "FunctionDefinition";
    protected const string ExpressionStatement = "ExpressionStatement";
    protected const string ReturnStatement = "ReturnStatement";
    protected const string VariableDeclarationStatement = "VariableDeclarationStatement";
    protected const string VariableDeclaration = "VariableDeclaration";
    protected const string FunctionCall = "FunctionCall";
    protected const string BinaryOperation = "BinaryOperation";
    protected const string IfStatement = "IfStatement";
    protected const string TupleExpression = "TupleExpression";
    protected const string Identifier = "Identifier";
    protected const string MemberAccess = "MemberAccess";
    protected const string IndexAccess = "IndexAccess";
    
    private const string StateVariableDeclaration = "StateVariableDeclaration";

    private readonly Dictionary<(string ContractName, string VariableName), (string VariableType, string VariableVisibility)> Locals;

    protected VariableFiltering()
    {
        Locals = new Dictionary<(string, string), (string, string)>();
    }

    protected abstract IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex,
        int? ArgumentIndex), string>> CheckStatementsForVariableName(
        List<JToken> statements, string variableName, int contractPosition, string functionName, int functionPosition);
    
    protected abstract IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex,
        int? ArgumentIndex), string>> CheckStatementForVariableName(
        JToken statement, string variableName, int contractPosition, string functionName, int functionPosition,
        int statementPosition);

    protected abstract IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int?
        SubStatementIndex,
        int? ArgumentIndex), string>> CheckStatementsForVariableType(
        List<JToken> statements, string variableType, string contractName, int contractPosition, string functionName,
        int functionPosition);

    protected abstract IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex,
        int? ArgumentIndex), string>> CheckStatementForVariableType(
        JToken statement, string variableType, string contractName, int contractPosition, string functionName,
        int functionPosition, int statementPosition);

    protected abstract
        IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex, int?
            ArgumentIndex), string>> CheckStatementsForVariableVisibility(
            List<JToken> statements, string variableVisibility, string contractName, int contractPosition,
            string functionName, int functionPosition);

    protected abstract
        IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex, int?
            ArgumentIndex), string>> CheckStatementForVariableVisibility(
            JToken statement, string variableVisibility, string contractName, int contractPosition, string functionName,
            int functionPosition, int statementPosition);
     
    /// <summary>
    /// Filter any form of variable interaction based on the name of the variable
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="variableName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterVariableInteractionByVariableName(JToken jToken, string variableName)
    {
        if (string.IsNullOrWhiteSpace(variableName))
            throw new ArgumentNullException(nameof(variableName));

        var interestedStatements = new Dictionary<(int, int, int, int?, int?), string>();

        var children = jToken["children"] as JArray;

        var childPosition = 0;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && !child["kind"].Matches("interface"))
            {
                var subNodes = child["subNodes"].ToSafeList();

                var subNodePosition = 0;
                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse())
                    {
                        var statements = subNode["body"]?["statements"].ToSafeList();
                        foreach (var (key, value) in CheckStatementsForVariableName(statements, variableName, childPosition, subNode["name"].Value<string>(), subNodePosition))
                        {
                            interestedStatements.Add(key, value);
                        }
                    }

                    subNodePosition++;
                }
            }

            childPosition++;
        }

        return new SelectionResult
        {
            InterestedStatements = interestedStatements
        };
    }

    /// <summary>
    /// Filter any form of variable interaction based on the type of the variable
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="variableType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterVariableInteractionByVariableType(JToken jToken, string variableType)
    {
        if (string.IsNullOrWhiteSpace(variableType))
            throw new ArgumentNullException(nameof(variableType));

        var interestedStatements = new Dictionary<(int, int, int, int?, int?), string>();

        var children = jToken["children"] as JArray;

        var childPosition = 0;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && !child["kind"].Matches("interface"))
            {
                var subNodes = child["subNodes"].ToSafeList();

                var subNodePosition = 0;
                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse())
                    {
                        var statements = subNode["body"]?["statements"].ToSafeList();
                        foreach (var (key, value) in CheckStatementsForVariableType(statements, variableType, child["name"].ToString(), childPosition, subNode["name"].Value<string>(), subNodePosition))
                        {
                            interestedStatements.Add(key, value);
                        }
                    }

                    subNodePosition++;
                }
            }

            childPosition++;
        }

        return new SelectionResult
        {
            InterestedStatements = interestedStatements
        };
    }

    /// <summary>
    /// Filter any form of variable interaction based on the visibility of the variable
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="variableVisibility"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public SelectionResult FilterVariableInteractionByVariableVisibility(JToken jToken, string variableVisibility)
    {
        if (string.IsNullOrWhiteSpace(variableVisibility))
            throw new ArgumentNullException(nameof(variableVisibility));

        var interestedStatements = new Dictionary<(int, int, int, int?, int?), string>();

        var children = jToken["children"] as JArray;

        var childPosition = 0;
        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && !child["kind"].Matches("interface"))
            {
                var subNodes = child["subNodes"].ToSafeList();

                var subNodePosition = 0;
                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(FunctionDefinition) && subNode["isConstructor"].IsFalse())
                    {
                        var statements = subNode["body"]?["statements"].ToSafeList();
                        foreach (var (key, value) in CheckStatementsForVariableVisibility(statements, variableVisibility, child["name"].ToString(), childPosition, subNode["name"].Value<string>(), subNodePosition))
                        {
                            interestedStatements.Add(key, value);
                        }
                    }

                    subNodePosition++;
                }
            }

            childPosition++;
        }

        return new SelectionResult
        {
            InterestedStatements = interestedStatements
        };
    }

    /// <summary>
    /// Filter any form of variable interaction based on the access key provided
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="accessKey"></param>
    /// <returns></returns>
    public SelectionResult FilterVariableInteractionByVariableAccessKey(JToken jToken, string accessKey)
    {
        // TODO - FilterVariableGettersByAccessKey
        throw new NotImplementedException();
    }

    public void LoadLocals(JToken source)
    {
        var children = source["children"] as JArray;

        foreach (var child in children.Children())
        {
            if (child["type"].Matches(ContractDefinition) && !child["kind"].Matches("interface"))
            {
                var contractName = child["name"].ToString();
                var subNodes = child["subNodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["type"].Matches(StateVariableDeclaration))
                    {
                        var variables = subNode["variables"].ToSafeList();

                        foreach (var variable in variables)
                        {
                            var variableName = variable["name"].ToString();
                            var variableType = variable["typeName"]["name"] == null ? variable["typeName"]["namePath"].ToString() : variable["typeName"]["name"].ToString();
                            var variableVisibility = variable["visibility"].ToString();

                            Locals.Add((contractName, variableName), (variableType, variableVisibility));
                        }
                    }
                }
            }
        }
    }

    protected bool IsVariableTypeMatch(string contractName, string variableName, string variableType)
    {
        return Locals.ContainsKey((contractName, variableName)) &&
               Locals[(contractName, variableName)].VariableType == variableType;
    }

    protected bool IsVariableTypeMatch(string contractName, JToken variableName, string variableType)
    {
        return variableName?.Value<string>() != null &&
               IsVariableTypeMatch(contractName, variableName.Value<string>(), variableType);
    }

    protected bool IsVariableVisibilityMatch(string contractName, string variableName, string variableVisibility)
    {
        return Locals.ContainsKey((contractName, variableName)) && (
               Locals[(contractName, variableName)].VariableVisibility == "default" && variableVisibility == "internal" ||
               Locals[(contractName, variableName)].VariableVisibility == variableVisibility);
    }

    protected bool IsVariableVisibilityMatch(string contractName, JToken variableName, string variableVisibility)
    {
        return variableName?.Value<string>() != null &&
               IsVariableVisibilityMatch(contractName, variableName.Value<string>(), variableVisibility);
    }
}