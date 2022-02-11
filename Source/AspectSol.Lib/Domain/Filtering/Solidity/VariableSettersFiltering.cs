using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering.Solidity;

public class VariableSettersFiltering : VariableFiltering
{
    protected override IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex, int? ArgumentIndex), string>> CheckStatementsForVariableName(
        List<JToken> statements, string variableName, int contractPosition, string functionName, int functionPosition)
    {
        var statementPosition = 0;
        foreach (var statement in statements)
        {
            foreach (var interestedStatement in CheckStatementForVariableName(statement, variableName, contractPosition, functionName, functionPosition,
                         statementPosition))
            {
                yield return interestedStatement;
            }

            statementPosition++;
        }
    }

    protected override IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex, int? ArgumentIndex), string>> CheckStatementForVariableName(
        JToken statement, string variableName, int contractPosition, string functionName, int functionPosition, int statementPosition)
    {
        // 1.   If the statement is a variable declaration such as 'var test = var1', then we
        //      either check if we are looking for a wildcard or that the variable name matches
        if (statement["type"].Matches(VariableDeclarationStatement))
        {
            var variables = statement["variables"].ToSafeList();

            foreach (var variable in variables)
            {
                if (variable["type"].Matches(VariableDeclaration) &&
                    variableName.Equals(Wildcard) || variable["name"].Matches(variableName))
                {
                    yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
                    break;
                }
            }
        }

        // 2.   If the statement is an if statement we need to iterate over inner statements
        else if (statement["type"].Matches(IfStatement))
        {
            var ifElseCount = 0;

            var ifStatementTrueStatements = statement["trueBody"]?["statements"].ToSafeList();
            foreach (var (key, value) in CheckStatementsForVariableName(ifStatementTrueStatements,
                         variableName, contractPosition, functionName, functionPosition))
            {
                ifElseCount += key.ArgumentIndex + 1 ?? 0;

                yield return new KeyValuePair<(int, int, int, int?, int?), string>(
                    (key.ContractIndex, key.FunctionIndex, statementPosition, key.SubStatementIndex ?? key.StatementIndex, ifElseCount), value);
            }

            if (statement["falseBody"].IsValueNullOrWhitespace() && statement["falseBody"]["type"].Matches("Block"))
            {
                var ifStatementFalseStatements = statement["falseBody"]?["statements"].ToSafeList();
                foreach (var (key, value) in CheckStatementsForVariableName(ifStatementFalseStatements,
                             variableName, contractPosition, functionName, functionPosition))
                {
                    ifElseCount += key.ArgumentIndex + 1 ?? 0;

                    yield return new KeyValuePair<(int, int, int, int?, int?), string>(
                        (key.ContractIndex, key.FunctionIndex, statementPosition, key.SubStatementIndex ?? key.StatementIndex, ifElseCount), value);
                }
            }
            else if (statement["falseBody"].IsValueNullOrWhitespace())
            {
                var ifStatementFalseStatements = statement["falseBody"];
                foreach (var (key, value) in CheckStatementForVariableName(ifStatementFalseStatements,
                             variableName, contractPosition, functionName, functionPosition, statementPosition))
                {
                    ifElseCount += key.ArgumentIndex + 1 ?? 0;

                    yield return new KeyValuePair<(int, int, int, int?, int?), string>(
                        (key.ContractIndex, key.FunctionIndex, statementPosition, key.SubStatementIndex, ifElseCount), value);
                }
            }
        }

        // 3.   If the statement is an expression statement such as 'test = var1 + var2'
        //      and either we are searching for a wildcard or left side variable matches variable name
        else if (statement["type"].Matches(ExpressionStatement) && statement["expression"]["type"].Matches(BinaryOperation) &&
                 (variableName.Equals(Wildcard) || statement["expression"]["left"]["name"].Matches(variableName)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }
    }

    protected override IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex, int? ArgumentIndex), string>> CheckStatementsForVariableType(
        List<JToken> statements, string variableType, string contractName, int contractPosition, string functionName, int functionPosition)
    {
        var statementPosition = 0;
        foreach (var statement in statements)
        {
            foreach (var interestedStatement in CheckStatementForVariableType(statement, variableType, contractName, contractPosition, functionName, functionPosition,
                         statementPosition))
            {
                yield return interestedStatement;
            }

            statementPosition++;
        }
    }

    protected override IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex, int? ArgumentIndex), string>> CheckStatementForVariableType(
        JToken statement, string variableType, string contractName, int contractPosition, string functionName, int functionPosition, int statementPosition)
    {
        // 1.   If the statement is a variable declaration such as 'var test = var1', then we
        //      either check if we are looking for a wildcard or that the variable name matches
        if (statement["type"].Matches(VariableDeclarationStatement))
        {
            var variables = statement["variables"].ToSafeList();

            foreach (var variable in variables)
            {
                if (variable["type"].Matches(VariableDeclaration) &&
                    variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, variable["name"], variableType))
                {
                    yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
                    break;
                }
            }
        }

        // 2.   If the statement is an if statement we need to iterate over inner statements
        else if (statement["type"].Matches(IfStatement))
        {
            var ifElseCount = 0;

            var ifStatementTrueStatements = statement["trueBody"]?["statements"].ToSafeList();
            foreach (var (key, value) in CheckStatementsForVariableType(ifStatementTrueStatements,
                         variableType, contractName, contractPosition, functionName, functionPosition))
            {
                ifElseCount += key.ArgumentIndex + 1 ?? 0;

                yield return new KeyValuePair<(int, int, int, int?, int?), string>(
                    (key.ContractIndex, key.FunctionIndex, statementPosition, key.SubStatementIndex ?? key.StatementIndex, ifElseCount), value);
            }

            if (statement["falseBody"].IsValueNullOrWhitespace() && statement["falseBody"]["type"].Matches("Block"))
            {
                var ifStatementFalseStatements = statement["falseBody"]?["statements"].ToSafeList();
                foreach (var (key, value) in CheckStatementsForVariableType(ifStatementFalseStatements,
                             variableType, contractName, contractPosition, functionName, functionPosition))
                {
                    ifElseCount += key.ArgumentIndex + 1 ?? 0;

                    yield return new KeyValuePair<(int, int, int, int?, int?), string>(
                        (key.ContractIndex, key.FunctionIndex, statementPosition, key.SubStatementIndex ?? key.StatementIndex, ifElseCount), value);
                }
            }
            else if (statement["falseBody"].IsValueNullOrWhitespace())
            {
                var ifStatementFalseStatements = statement["falseBody"];
                foreach (var (key, value) in CheckStatementForVariableType(ifStatementFalseStatements,
                             variableType, contractName, contractPosition, functionName, functionPosition, statementPosition))
                {
                    ifElseCount += key.ArgumentIndex + 1 ?? 0;

                    yield return new KeyValuePair<(int, int, int, int?, int?), string>(
                        (key.ContractIndex, key.FunctionIndex, statementPosition, key.SubStatementIndex, ifElseCount), value);
                }
            }
        }

        // 3.   If the statement is an expression statement such as 'test = var1 + var2'
        //      and either we are searching for a wildcard or left side variable matches variable name
        else if (statement["type"].Matches(ExpressionStatement) && statement["expression"]["type"].Matches(BinaryOperation) &&
                 (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, statement["expression"]["left"]["name"], variableType)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }
    }

    protected override IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex, int? ArgumentIndex), string>> CheckStatementsForVariableVisibility(
        List<JToken> statements, string variableVisibility, string contractName, int contractPosition, string functionName, int functionPosition)
    {
        var statementPosition = 0;
        foreach (var statement in statements)
        {
            foreach (var interestedStatement in CheckStatementForVariableVisibility(statement, variableVisibility, contractName, contractPosition, functionName, functionPosition,
                         statementPosition))
            {
                yield return interestedStatement;
            }

            statementPosition++;
        }
    }

    protected override IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex, int? ArgumentIndex), string>> CheckStatementForVariableVisibility(
        JToken statement, string variableVisibility, string contractName, int contractPosition, string functionName, int functionPosition, int statementPosition)
    {
        // 1.   If the statement is a variable declaration such as 'var test = var1', then we
        //      either check if we are looking for a wildcard or that the variable name matches
        if (statement["type"].Matches(VariableDeclarationStatement))
        {
            var variables = statement["variables"].ToSafeList();

            foreach (var variable in variables)
            {
                if (variable["type"].Matches(VariableDeclaration) &&
                    variableVisibility.Equals(Wildcard) || IsVariableVisibilityMatch(contractName, variable["name"], variableVisibility))
                {
                    yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
                    break;
                }
            }
        }

        // 2.   If the statement is an if statement we need to iterate over inner statements
        else if (statement["type"].Matches(IfStatement))
        {
            var ifElseCount = 0;

            var ifStatementTrueStatements = statement["trueBody"]?["statements"].ToSafeList();
            foreach (var (key, value) in CheckStatementsForVariableVisibility(ifStatementTrueStatements,
                         variableVisibility, contractName, contractPosition, functionName, functionPosition))
            {
                ifElseCount += key.ArgumentIndex + 1 ?? 0;

                yield return new KeyValuePair<(int, int, int, int?, int?), string>(
                    (key.ContractIndex, key.FunctionIndex, statementPosition, key.SubStatementIndex ?? key.StatementIndex, ifElseCount), value);
            }

            if (statement["falseBody"].IsValueNullOrWhitespace() && statement["falseBody"]["type"].Matches("Block"))
            {
                var ifStatementFalseStatements = statement["falseBody"]?["statements"].ToSafeList();
                foreach (var (key, value) in CheckStatementsForVariableVisibility(ifStatementFalseStatements,
                             variableVisibility, contractName, contractPosition, functionName, functionPosition))
                {
                    ifElseCount += key.ArgumentIndex + 1 ?? 0;

                    yield return new KeyValuePair<(int, int, int, int?, int?), string>(
                        (key.ContractIndex, key.FunctionIndex, statementPosition, key.SubStatementIndex ?? key.StatementIndex, ifElseCount), value);
                }
            }
            else if (statement["falseBody"].IsValueNullOrWhitespace())
            {
                var ifStatementFalseStatements = statement["falseBody"];
                foreach (var (key, value) in CheckStatementForVariableVisibility(ifStatementFalseStatements,
                             variableVisibility, contractName, contractPosition, functionName, functionPosition, statementPosition))
                {
                    ifElseCount += key.ArgumentIndex + 1 ?? 0;

                    yield return new KeyValuePair<(int, int, int, int?, int?), string>(
                        (key.ContractIndex, key.FunctionIndex, statementPosition, key.SubStatementIndex, ifElseCount), value);
                }
            }
        }

        // 3.   If the statement is an expression statement such as 'test = var1 + var2'
        //      and either we are searching for a wildcard or left side variable matches variable name
        else if (statement["type"].Matches(ExpressionStatement) && statement["expression"]["type"].Matches(BinaryOperation) &&
                 (variableVisibility.Equals(Wildcard) || IsVariableVisibilityMatch(contractName, statement["expression"]["left"]["name"], variableVisibility)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }
    }
}