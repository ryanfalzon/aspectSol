using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering.Solidity;

public abstract class VariableFiltering : IVariableInteractionFiltering
{
    protected const string ContractDefinition = "ContractDefinition";
    protected const string Wildcard = "*";
    protected const string FunctionDefinition = "FunctionDefinition";
    protected const string ModifierDefinition = "ModifierDefinition";
    protected const string ExpressionStatement = "ExpressionStatement";
    protected const string Return = "Return";
    protected const string VariableDeclaration = "VariableDeclaration";
    protected const string VariableDeclarationStatement = "VariableDeclarationStatement";
    protected const string FunctionCall = "FunctionCall";
    protected const string BinaryOperation = "BinaryOperation";
    protected const string Assignment = "Assignment";
    protected const string IfStatement = "IfStatement";
    protected const string TupleExpression = "TupleExpression";
    protected const string Identifier = "Identifier";
    protected const string MemberAccess = "MemberAccess";
    protected const string IndexAccess = "IndexAccess";
    
    private const string StateVariableDeclaration = "StateVariableDeclaration";

    private readonly Dictionary<(string ContractName, string VariableName), (string VariableType, string VariableVisibility)> _locals;

    protected VariableFiltering()
    {
        _locals = new Dictionary<(string, string), (string, string)>();
    }
    
    protected List<StatementFilteringResult> CheckStatementsForVariableName(List<JToken> statements, string variableName)
    {
        var statementFilteringResults = new List<StatementFilteringResult>();

        var statementIndex = 0;
        foreach (var statement in statements)
        {
            var statementFilteringResult = CheckStatementForVariableName(statement, variableName, statementIndex);
            if (statementFilteringResult != null) statementFilteringResults.Add(statementFilteringResult);

            statementIndex++;
        }

        return statementFilteringResults;
    }

    protected List<StatementFilteringResult> CheckStatementsForVariableType(List<JToken> statements, string contractName, string variableType)
    {
        var statementFilteringResults = new List<StatementFilteringResult>();

        var statementIndex = 0;
        foreach (var statement in statements)
        {
            var statementFilteringResult = CheckStatementForVariableType(statement, contractName, variableType, statementIndex);
            if (statementFilteringResult != null) statementFilteringResults.Add(statementFilteringResult);

            statementIndex++;
        }

        return statementFilteringResults;
    }

    protected List<StatementFilteringResult> CheckStatementsForVariableVisibility(List<JToken> statements, string contractName,
        string variableVisibility)
    {
        var statementFilteringResults = new List<StatementFilteringResult>();

        var statementIndex = 0;
        foreach (var statement in statements)
        {
            var statementFilteringResult = CheckStatementForVariableVisibility(statement, contractName, variableVisibility, statementIndex);
            if (statementFilteringResult != null) statementFilteringResults.Add(statementFilteringResult);

            statementIndex++;
        }

        return statementFilteringResults;
    }

    protected abstract StatementFilteringResult CheckStatementForVariableName(JToken statement, string variableName, int statementPosition);
    protected abstract StatementFilteringResult CheckStatementForVariableType(JToken statement, string contractName, string variableType, int statementPosition);
    protected abstract StatementFilteringResult CheckStatementForVariableVisibility(JToken statement, string contractName, string variableVisibility, int statementPosition);
     
    /// <summary>
    /// Filter any form of variable interaction based on the name of the variable
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="variableName"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public FilteringResult FilterVariableInteractionByVariableName(JToken jToken, string variableName)
    {
        if (string.IsNullOrWhiteSpace(variableName))
            throw new ArgumentNullException(nameof(variableName));

        var filteringResult = new FilteringResult();

        var children =  jToken["nodes"] as JArray ?? new JArray();

        foreach (var child in children.Children())
        {
            var contractName = child["name"]?.Value<string>() ?? string.Empty;
            
            if (child["nodeType"].Matches(ContractDefinition) && !child["contractKind"].Matches("interface"))
            {
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["nodeType"].Matches(FunctionDefinition) && (subNode["kind"].Matches("function") || subNode["kind"].Matches("constructor")) ||
                        subNode["nodeType"].Matches(ModifierDefinition))
                    {
                        var currentFunctionName = subNode["name"]?.Value<string>() ?? string.Empty;
                        
                        var statements = subNode["body"]?["statements"].ToSafeList();
                        var statementFilteringResults = CheckStatementsForVariableName(statements, variableName);
                        
                        filteringResult.AddStatement(contractName, currentFunctionName, statementFilteringResults);
                    }
                }
            }
        }

        return filteringResult;
    }

    /// <summary>
    /// Filter any form of variable interaction based on the type of the variable
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="variableType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public FilteringResult FilterVariableInteractionByVariableType(JToken jToken, string variableType)
    {
        if (string.IsNullOrWhiteSpace(variableType))
            throw new ArgumentNullException(nameof(variableType));

        var filteringResult = new FilteringResult();

        var children =  jToken["nodes"] as JArray ?? new JArray();

        foreach (var child in children.Children())
        {
            var contractName = child["name"]?.Value<string>() ?? string.Empty;
            
            if (child["nodeType"].Matches(ContractDefinition) && !child["contractKind"].Matches("interface"))
            {
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["nodeType"].Matches(FunctionDefinition) && (subNode["kind"].Matches("function") || subNode["kind"].Matches("constructor")) ||
                        subNode["nodeType"].Matches(ModifierDefinition))
                    {
                        var currentFunctionName = subNode["name"]?.Value<string>() ?? string.Empty;
                        
                        var statements = subNode["body"]?["statements"].ToSafeList();
                        var statementFilteringResults = CheckStatementsForVariableType(statements, contractName, variableType);
                        
                        filteringResult.AddStatement(contractName, currentFunctionName, statementFilteringResults);
                    }
                }
            }
        }

        return filteringResult;
    }

    /// <summary>
    /// Filter any form of variable interaction based on the visibility of the variable
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="variableVisibility"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public FilteringResult FilterVariableInteractionByVariableVisibility(JToken jToken, string variableVisibility)
    {
        if (string.IsNullOrWhiteSpace(variableVisibility))
            throw new ArgumentNullException(nameof(variableVisibility));

        var filteringResult = new FilteringResult();

        var children =  jToken["nodes"] as JArray ?? new JArray();

        foreach (var child in children.Children())
        {
            var contractName = child["name"]?.Value<string>() ?? string.Empty;
            
            if (child["nodeType"].Matches(ContractDefinition) && !child["contractKind"].Matches("interface"))
            {
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["nodeType"].Matches(FunctionDefinition) && (subNode["kind"].Matches("function") || subNode["kind"].Matches("constructor")) ||
                        subNode["nodeType"].Matches(ModifierDefinition))
                    {
                        var currentFunctionName = subNode["name"]?.Value<string>() ?? string.Empty;
                        
                        var statements = subNode["body"]?["statements"].ToSafeList();
                        var statementFilteringResults = CheckStatementsForVariableVisibility(statements, contractName, variableVisibility);
                        
                        filteringResult.AddStatement(contractName, currentFunctionName, statementFilteringResults);
                    }
                }
            }
        }

        return filteringResult;
    }

    /// <summary>
    /// Filter any form of variable interaction based on the access key provided
    /// </summary>
    /// <param name="jToken"></param>
    /// <param name="accessKey"></param>
    /// <returns></returns>
    public FilteringResult FilterVariableInteractionByVariableAccessKey(JToken jToken, string accessKey)
    {
        // TODO - FilterVariableGettersByAccessKey
        throw new NotImplementedException();
    }

    public void LoadLocals(JToken source)
    {
        var children = source["nodes"] as JArray ?? new JArray();

        foreach (var child in children.Children())
        {
            if (child["nodeType"].Matches(ContractDefinition) && !child["contractKind"].Matches("interface"))
            {
                var contractName = child["name"]?.Value<string>() ?? string.Empty;
                var subNodes = child["nodes"].ToSafeList();

                foreach (var subNode in subNodes)
                {
                    if (subNode["nodeType"].Matches(VariableDeclaration))
                    {
                        var variableName = subNode["name"]?.Value<string>();
                        var variableType = subNode["typeDescriptions"]?["typeString"]?.Value<string>();
                        var variableVisibility = subNode["visibility"]?.Value<string>();

                        _locals.Add((contractName, variableName), (variableType, variableVisibility));
                    }
                }
            }
        }
    }

    protected bool IsVariableTypeMatch(string contractName, string variableName, string variableType)
    {
        return _locals.ContainsKey((contractName, variableName)) &&
               _locals[(contractName, variableName)].VariableType == variableType;
    }

    protected bool IsVariableTypeMatch(string contractName, JToken variableName, string variableType)
    {
        return variableName?.Value<string>() != null &&
               IsVariableTypeMatch(contractName, variableName.Value<string>(), variableType);
    }

    protected bool IsVariableVisibilityMatch(string contractName, string variableName, string variableVisibility)
    {
        return _locals.ContainsKey((contractName, variableName)) && (
               _locals[(contractName, variableName)].VariableVisibility == "default" && variableVisibility == "internal" ||
               _locals[(contractName, variableName)].VariableVisibility == variableVisibility);
    }

    protected bool IsVariableVisibilityMatch(string contractName, JToken variableName, string variableVisibility)
    {
        return variableName?.Value<string>() != null &&
               IsVariableVisibilityMatch(contractName, variableName.Value<string>(), variableVisibility);
    }
}