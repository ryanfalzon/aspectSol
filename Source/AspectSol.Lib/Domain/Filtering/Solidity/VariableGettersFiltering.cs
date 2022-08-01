using AspectSol.Lib.Domain.Filtering.FilteringResults;
using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering.Solidity;

public class VariableGettersFiltering : VariableFiltering
{
    protected override StatementFilteringResult CheckStatementForVariableName(JToken statement, string variableName, int statementPosition)
    {
        // 1.   If the statement is a variable declaration such as 'var test = var1', then we either check if we are looking for a wildcard or that the
        //      variable name matches
        if (statement["nodeType"].Matches(VariableDeclarationStatement) &&
            (variableName.Equals(Wildcard) || statement["initialValue"]["name"].Matches(variableName)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 2.   If the statement is a variable declaration through a binary operation such as 'var test = var1 == var2' then we need to check if either we are
        //      looking for a wildcard or either the left side or the right side variables match the variable name
        if (statement["nodeType"].Matches(VariableDeclarationStatement) && statement["initialValue"]["nodeType"].Matches(BinaryOperation) &&
            (variableName.Equals(Wildcard) || (statement["initialValue"]?["leftExpression"]?["name"].Matches(variableName) ?? false) ||
                (statement["initialValue"]?["rightExpression"]?["name"].Matches(variableName) ?? false)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 3.   If the statement is an expression statement such as 'test = var1', then we either check if we are looking for a wildcard or that the variable
        //      name matches
        if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(Assignment) &&
            (variableName.Equals(Wildcard) || (statement["expression"]?["rightHandSide"]?["name"].Matches(variableName) ?? false)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 4.   If the statement is an expression statement through an assignment such as 'test = var1 == var2' then we need to check if either we are looking
        //      for a wildcard or either the left side or the right side variables match the variable name
        if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(Assignment) &&
            (statement["expression"]?["rightHandSide"]?["nodeType"].Matches(BinaryOperation) ?? false) && (variableName.Equals(Wildcard) ||
                (statement["expression"]?["rightHandSide"]?["rightExpression"]?["name"].Matches(variableName) ?? false) ||
                (statement["expression"]?["rightHandSide"]?["leftExpression"]?["name"].Matches(variableName) ?? false)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 5.   If the statement is an expression statement and a function call such as 'storageA.store(numberB)' and either we are searching for a wildcard or
        //      one of the variables passed to the function call match the variable name
        if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(FunctionCall))
        {
            var arguments = statement["expression"]["arguments"].ToSafeList();

            foreach (var argument in arguments)
            {
                if (variableName.Equals(Wildcard) || argument["name"].Matches(variableName))
                {
                    return new StatementFilteringResult(statementPosition);
                }
            }

            return null;
        }

        // 6.   If the statement is a simple return statement such as 'return variableName' and either we are searching for a wildcard or the variable
        //      name matches
        if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(Identifier) &&
            (variableName.Equals(Wildcard) || statement["expression"]["name"].Matches(variableName)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 7.   If the statement is a tuple return statement such as 'return (var1, var2)' then we need to check all components within the tuple and if either
        //      we are either searching for a wildcard, or at least one of the variable name matches
        if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(TupleExpression))
        {
            var components = statement["expression"]["components"].ToSafeList();

            foreach (var component in components)
            {
                if (variableName.Equals(Wildcard) || component["name"].Matches(variableName))
                {
                    return new StatementFilteringResult(statementPosition);
                }
            }

            return null;
        }

        // 8.   If the statement is a return statement through a binary operation such as 'return var1 + var2' then we need to check all components within the
        //      tuple and if either we are either searching for a wildcard, or at least one of the variable name matches
        if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(BinaryOperation) && (variableName.Equals(Wildcard) ||
                (statement["expression"]?["leftExpression"]?["name"].Matches(variableName) ?? false) ||
                (statement["expression"]?["rightExpression"]?["name"].Matches(variableName) ?? false)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 9.   If the statement is an if statement and either we are looking for a wildcard or the left or right side of the condition have a variable
        //      matching the variable name
        if (statement["nodeType"].Matches(IfStatement) && statement["condition"]["nodeType"].Matches(BinaryOperation) && (variableName.Equals(Wildcard) ||
                (statement["condition"]?["leftExpression"]?["name"].Matches(variableName) ?? false) ||
                (statement["condition"]?["rightExpression"]?["name"].Matches(variableName) ?? false)))
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
        if (statement["nodeType"].Matches(VariableDeclarationStatement) &&
            (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, statement["initialValue"]["name"], variableType)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 2.   If the statement is a variable declaration through a binary operation such as 'var test = var1 == var2' then we need to check if either we are
        //      looking for a wildcard or either the left side or the right side variables match the variable name
        if (statement["nodeType"].Matches(VariableDeclarationStatement) && statement["initialValue"]["nodeType"].Matches(BinaryOperation) &&
            (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, statement["initialValue"]?["leftExpression"]?["name"], variableType) ||
                IsVariableTypeMatch(contractName, statement["initialValue"]?["rightExpression"]?["name"], variableType)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 3.   If the statement is an expression statement such as 'test = var1', then we either check if we are looking for a wildcard or that the variable
        //      name matches
        if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(Assignment) &&
            (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, statement["expression"]?["rightHandSide"]?["name"], variableType)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 4.   If the statement is an expression statement through an assignment such as 'test = var1 == var2' then we need to check if either we are looking
        //      for a wildcard or either the left side or the right side variables match the variable name
        if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(Assignment) &&
            (statement["expression"]?["rightHandSide"]?["nodeType"].Matches(BinaryOperation) ?? false) && (variableType.Equals(Wildcard) ||
                IsVariableTypeMatch(contractName, statement["expression"]?["rightHandSide"]?["rightExpression"]?["name"], variableType) ||
                IsVariableTypeMatch(contractName, statement["expression"]?["rightHandSide"]?["leftExpression"]?["name"], variableType)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 5.   If the statement is an expression statement and a function call such as 'storageA.store(numberB)' and either we are searching for a wildcard or
        //      one of the variables passed to the function call match the variable name
        if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(FunctionCall))
        {
            var arguments = statement["expression"]["arguments"].ToSafeList();

            foreach (var argument in arguments)
            {
                if (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, argument["name"], variableType))
                {
                    return new StatementFilteringResult(statementPosition);
                }
            }

            return null;
        }

        // 6.   If the statement is a simple return statement such as 'return variableName' and either we are searching for a wildcard or the variable
        //      name matches
        if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(Identifier) &&
            (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, statement["expression"]["name"], variableType)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 7.   If the statement is a tuple return statement such as 'return (var1, var2)' then we need to check all components within the tuple and if either
        //      we are either searching for a wildcard, or at least one of the variable name matches
        if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(TupleExpression))
        {
            var components = statement["expression"]["components"].ToSafeList();

            foreach (var component in components)
            {
                if (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, component["name"], variableType))
                {
                    return new StatementFilteringResult(statementPosition);
                }
            }

            return null;
        }

        // 8.   If the statement is a return statement through a binary operation such as 'return var1 + var2' then we need to check all components within the
        //      tuple and if either we are either searching for a wildcard, or at least one of the variable name matches
        if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(BinaryOperation) && (variableType.Equals(Wildcard) ||
                IsVariableTypeMatch(contractName, statement["expression"]?["leftExpression"]?["name"], variableType) ||
                IsVariableTypeMatch(contractName, statement["expression"]?["rightExpression"]?["name"], variableType)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 9.   If the statement is an if statement and either we are looking for a wildcard or the left or right side of the condition have a variable
        //      matching the variable name
        if (statement["nodeType"].Matches(IfStatement) && statement["condition"]["nodeType"].Matches(BinaryOperation) && (variableType.Equals(Wildcard) ||
                IsVariableTypeMatch(contractName, statement["condition"]?["leftExpression"]?["name"], variableType) ||
                IsVariableTypeMatch(contractName, statement["condition"]?["rightExpression"]?["name"], variableType)))
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
        if (statement["nodeType"].Matches(VariableDeclarationStatement) &&
            (variableVisibility.Equals(Wildcard) || IsVariableTypeMatch(contractName, statement["initialValue"]["name"], variableVisibility)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 2.   If the statement is a variable declaration through a binary operation such as 'var test = var1 == var2' then we need to check if either we are
        //      looking for a wildcard or either the left side or the right side variables match the variable name
        if (statement["nodeType"].Matches(VariableDeclarationStatement) && statement["initialValue"]["nodeType"].Matches(BinaryOperation) &&
            (variableVisibility.Equals(Wildcard) ||
                IsVariableVisibilityMatch(contractName, statement["initialValue"]?["leftExpression"]?["name"], variableVisibility) ||
                IsVariableVisibilityMatch(contractName, statement["initialValue"]?["rightExpression"]?["name"], variableVisibility)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 3.   If the statement is an expression statement such as 'test = var1', then we either check if we are looking for a wildcard or that the variable
        //      name matches
        if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(Assignment) &&
            (variableVisibility.Equals(Wildcard) ||
                IsVariableVisibilityMatch(contractName, statement["expression"]?["rightHandSide"]?["name"], variableVisibility)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 4.   If the statement is an expression statement through an assignment such as 'test = var1 == var2' then we need to check if either we are looking
        //      for a wildcard or either the left side or the right side variables match the variable name
        if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(Assignment) &&
            (statement["expression"]?["rightHandSide"]?["nodeType"].Matches(BinaryOperation) ?? false) && (variableVisibility.Equals(Wildcard) ||
                IsVariableVisibilityMatch(contractName, statement["expression"]?["rightHandSide"]?["rightExpression"]?["name"], variableVisibility) ||
                IsVariableVisibilityMatch(contractName, statement["expression"]?["rightHandSide"]?["leftExpression"]?["name"], variableVisibility)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 5.   If the statement is an expression statement and a function call such as 'storageA.store(numberB)' and either we are searching for a wildcard or
        //      one of the variables passed to the function call match the variable name
        if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(FunctionCall))
        {
            var arguments = statement["expression"]["arguments"].ToSafeList();

            foreach (var argument in arguments)
            {
                if (variableVisibility.Equals(Wildcard) || IsVariableVisibilityMatch(contractName, argument["name"], variableVisibility))
                {
                    return new StatementFilteringResult(statementPosition);
                }
            }

            return null;
        }

        // 6.   If the statement is a simple return statement such as 'return variableName' and either we are searching for a wildcard or the variable
        //      name matches
        if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(Identifier) &&
            (variableVisibility.Equals(Wildcard) || IsVariableVisibilityMatch(contractName, statement["expression"]["name"], variableVisibility)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 7.   If the statement is a tuple return statement such as 'return (var1, var2)' then we need to check all components within the tuple and if either
        //      we are either searching for a wildcard, or at least one of the variable name matches
        if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(TupleExpression))
        {
            var components = statement["expression"]["components"].ToSafeList();

            foreach (var component in components)
            {
                if (variableVisibility.Equals(Wildcard) || IsVariableVisibilityMatch(contractName, component["name"], variableVisibility))
                {
                    return new StatementFilteringResult(statementPosition);
                }
            }

            return null;
        }

        // 8.   If the statement is a return statement through a binary operation such as 'return var1 + var2' then we need to check all components within the
        //      tuple and if either we are either searching for a wildcard, or at least one of the variable name matches
        if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(BinaryOperation) &&
            (variableVisibility.Equals(Wildcard) ||
                IsVariableVisibilityMatch(contractName, statement["expression"]?["leftExpression"]?["name"], variableVisibility) ||
                IsVariableVisibilityMatch(contractName, statement["expression"]?["rightExpression"]?["name"], variableVisibility)))
        {
            return new StatementFilteringResult(statementPosition);
        }

        // 9.   If the statement is an if statement and either we are looking for a wildcard or the left or right side of the condition have a variable
        //      matching the variable name
        if (statement["nodeType"].Matches(IfStatement) && statement["condition"]["nodeType"].Matches(BinaryOperation) &&
            (variableVisibility.Equals(Wildcard) ||
                IsVariableVisibilityMatch(contractName, statement["condition"]?["leftExpression"]?["name"], variableVisibility) ||
                IsVariableVisibilityMatch(contractName, statement["condition"]?["rightExpression"]?["name"], variableVisibility)))
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