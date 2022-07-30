using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering.Solidity;

public class VariableSettersFiltering : VariableFiltering
{

    protected override StatementFilteringResult CheckStatementForVariableName(JToken statement, string variableName, int statementPosition)
    {
        // 1.   If the statement is a variable declaration such as 'var test = var1', then we either check if we are looking for a wildcard or that the
        //      variable name matches
        if (statement["nodeType"].Matches(VariableDeclarationStatement))
        {
            var declarations = statement["declarations"].ToSafeList();

            foreach (var declaration in declarations)
            {
                if (variableName.Equals(Wildcard) || declaration["name"].Matches(variableName))
                {
                    return new StatementFilteringResult(statementPosition);
                }
            }

            return null;
        }

        // 2.   If the statement is an expression statement such as 'test = var1', then we either check if we are looking for a wildcard or that the variable
        //      name matches
        if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(Assignment) &&
                 (variableName.Equals(Wildcard) || (statement["expression"]?["leftHandSide"]?["name"].Matches(variableName) ?? false)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 3.   If the statement is an if statement we need to iterate over all statements
        if (statement["nodeType"].Matches(IfStatement))
        {
            var ifElseFilteringResults = new List<StatementFilteringResult>();
            
            var ifStatementTrueFilteringResults = new List<StatementFilteringResult>();
            var ifStatementTrueStatements = statement["trueBody"]?["statements"].ToSafeList();
            foreach (var statementFilteringResult in CheckStatementsForVariableName(ifStatementTrueStatements, variableName))
            {
                ifStatementTrueFilteringResults.Add(statementFilteringResult);
            }

            ifElseFilteringResults.AddRange(ifStatementTrueFilteringResults);

            if (!statement["falseBody"].IsValueNullOrWhitespace() && statement["falseBody"]["nodeType"].Matches("Block"))
            {
                var ifStatementFalseFilteringResults = new List<StatementFilteringResult>();
                var ifStatementFalseStatements = statement["falseBody"]?["statements"].ToSafeList();
                foreach (var statementFilteringResult in CheckStatementsForVariableName(ifStatementFalseStatements, variableName))
                {
                    ifStatementFalseFilteringResults.Add(statementFilteringResult);
                }

                ifElseFilteringResults.AddRange(ifStatementFalseFilteringResults);
            }
            else if (!statement["falseBody"].IsValueNullOrWhitespace())
            {
                var ifStatementFalseFilteringResults = new List<StatementFilteringResult>();
                var ifStatementFalseStatements = statement["falseBody"].ToSafeList();
                foreach (var statementFilteringResult in CheckStatementsForVariableName(ifStatementFalseStatements, variableName))
                {
                    ifStatementFalseFilteringResults.Add(statementFilteringResult);
                }

                ifElseFilteringResults.AddRange(ifStatementFalseFilteringResults);
            }

            return new StatementFilteringResult(statementPosition, ifElseFilteringResults);
        }

        return null;
    }

    protected override StatementFilteringResult CheckStatementForVariableType(JToken statement, string contractName, string variableType, int statementPosition)
    {
        // 1.   If the statement is a variable declaration such as 'var test = var1', then we either check if we are looking for a wildcard or that the
        //      variable name matches
        if (statement["nodeType"].Matches(VariableDeclarationStatement))
        {
            var declarations = statement["declarations"].ToSafeList();

            foreach (var declaration in declarations)
            {
                if (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, declaration["name"], variableType))
                {
                    return new StatementFilteringResult(statementPosition);
                }
            }

            return null;
        }

        // 2.   If the statement is an expression statement such as 'test = var1', then we either check if we are looking for a wildcard or that the variable
        //      name matches
        if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(Assignment) &&
                 (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, statement["expression"]?["leftHandSide"]?["name"], variableType)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 3.   If the statement is an if statement we need to iterate over all statements
        if (statement["nodeType"].Matches(IfStatement))
        {
            var ifElseFilteringResults = new List<StatementFilteringResult>();

            var ifStatementTrueFilteringResults = new List<StatementFilteringResult>();
            var ifStatementTrueStatements = statement["trueBody"]?["statements"].ToSafeList();
            foreach (var statementFilteringResult in CheckStatementsForVariableType(ifStatementTrueStatements, contractName, variableType))
            {
                ifStatementTrueFilteringResults.Add(statementFilteringResult);
            }

            ifElseFilteringResults.AddRange(ifStatementTrueFilteringResults);

            if (!statement["falseBody"].IsValueNullOrWhitespace() && statement["falseBody"]["nodeType"].Matches("Block"))
            {
                var ifStatementFalseFilteringResults = new List<StatementFilteringResult>();
                var ifStatementFalseStatements = statement["falseBody"]?["statements"].ToSafeList();
                foreach (var statementFilteringResult in CheckStatementsForVariableType(ifStatementFalseStatements, contractName, variableType))
                {
                    ifStatementFalseFilteringResults.Add(statementFilteringResult);
                }

                ifElseFilteringResults.AddRange(ifStatementFalseFilteringResults);
            }
            else if (!statement["falseBody"].IsValueNullOrWhitespace())
            {
                var ifStatementFalseFilteringResults = new List<StatementFilteringResult>();
                var ifStatementFalseStatements = statement["falseBody"].ToSafeList();
                foreach (var statementFilteringResult in CheckStatementsForVariableType(ifStatementFalseStatements, contractName, variableType))
                {
                    ifStatementFalseFilteringResults.Add(statementFilteringResult);
                }

                ifElseFilteringResults.AddRange(ifStatementFalseFilteringResults);
            }

            return new StatementFilteringResult(statementPosition, ifElseFilteringResults);
        }

        return null;
    }

    protected override StatementFilteringResult CheckStatementForVariableVisibility(JToken statement, string contractName, string variableVisibility, int statementPosition)
    {
        // 1.   If the statement is a variable declaration such as 'var test = var1', then we either check if we are looking for a wildcard or that the
        //      variable name matches
        if (statement["nodeType"].Matches(VariableDeclarationStatement))
        {
            var declarations = statement["declarations"].ToSafeList();

            foreach (var declaration in declarations)
            {
                if (variableVisibility.Equals(Wildcard) || IsVariableVisibilityMatch(contractName, declaration["name"], variableVisibility))
                {
                    return new StatementFilteringResult(statementPosition);
                }
            }

            return null;
        }

        // 2.   If the statement is an expression statement such as 'test = var1', then we either check if we are looking for a wildcard or that the variable
        //      name matches
        if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(Assignment) &&
                 (variableVisibility.Equals(Wildcard) || IsVariableVisibilityMatch(contractName, statement["expression"]?["leftHandSide"]?["name"], variableVisibility)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 3.   If the statement is an if statement we need to iterate over all statements
        if (statement["nodeType"].Matches(IfStatement))
        {
            var ifElseFilteringResults = new List<StatementFilteringResult>();

            var ifStatementTrueFilteringResults = new List<StatementFilteringResult>();
            var ifStatementTrueStatements = statement["trueBody"]?["statements"].ToSafeList();
            foreach (var statementFilteringResult in CheckStatementsForVariableVisibility(ifStatementTrueStatements, contractName, variableVisibility))
            {
                ifStatementTrueFilteringResults.Add(statementFilteringResult);
            }

            ifElseFilteringResults.AddRange(ifStatementTrueFilteringResults);

            if (!statement["falseBody"].IsValueNullOrWhitespace() && statement["falseBody"]["nodeType"].Matches("Block"))
            {
                var ifStatementFalseFilteringResults = new List<StatementFilteringResult>();
                var ifStatementFalseStatements = statement["falseBody"]?["statements"].ToSafeList();
                foreach (var statementFilteringResult in CheckStatementsForVariableVisibility(ifStatementFalseStatements, contractName, variableVisibility))
                {
                    ifStatementFalseFilteringResults.Add(statementFilteringResult);
                }

                ifElseFilteringResults.AddRange(ifStatementFalseFilteringResults);
            }
            else if (!statement["falseBody"].IsValueNullOrWhitespace())
            {
                var ifStatementFalseFilteringResults = new List<StatementFilteringResult>();
                var ifStatementFalseStatements = statement["falseBody"].ToSafeList();
                foreach (var statementFilteringResult in CheckStatementsForVariableVisibility(ifStatementFalseStatements, contractName, variableVisibility))
                {
                    ifStatementFalseFilteringResults.Add(statementFilteringResult);
                }

                ifElseFilteringResults.AddRange(ifStatementFalseFilteringResults);
            }

            return new StatementFilteringResult(statementPosition, ifElseFilteringResults);
        }

        return null;
    }
}