using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering.Solidity;

public class VariableGettersFiltering : VariableFiltering
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
        // 1.   If the statement is a simple return statement such as 'return variableName'
        //      and either we are searching for a wildcard or the variable name matches
        if (statement["type"].Matches(ReturnStatement) &&
            (variableName.Equals(Wildcard) || statement["expression"]["name"].Matches(variableName)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 2.   If the statement is a tuple return statement such as 'return (var1, var2)'
        //      then we need to check all components within the tuple and if either we are
        //      either searching for a wildcard, or at least one of the variable name matches
        else if (statement["type"].Matches(ReturnStatement) && statement["expression"]["type"].Matches(TupleExpression))
        {
            var components = statement["expression"]["components"].ToSafeList();

            var componentPosition = 0;
            foreach (var component in components)
            {
                if (variableName.Equals(Wildcard) || component["name"].Matches(variableName))
                {
                    yield return new((contractPosition, functionPosition, statementPosition, null, componentPosition), functionName);
                    break;
                }

                componentPosition++;
            }
        }

        // 3.   If the statement is a variable declaration through a binary operation such as
        //      'var test = var1 == var2' then we need to check if either we are looking for a
        //      wildcard or either the left side or the right side variables match the variable name
        else if (statement["type"].Matches(VariableDeclarationStatement) && statement["initialValue"]["type"].Matches(BinaryOperation)
                 && (variableName.Equals(Wildcard) || statement["initialValue"]["left"]["name"].Matches(variableName)
                                                   || statement["initialValue"]["right"]["name"].Matches(variableName)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 4.   If the statement is a variable declaration such as 'var test = var1', then we
        //      either check if we are looking for a wildcard or that the variable name matches
        else if (statement["type"].Matches(VariableDeclarationStatement) &&
                 (variableName.Equals(Wildcard) || statement["initialValue"]["name"].Matches(variableName)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 5.   If the statement is an if statement and either we are looking for a wildcard
        //      or the left or right side of the condition have a variable matching the variable name
        else if (statement["type"].Matches(IfStatement) && statement["condition"]["type"].Matches(BinaryOperation) &&
                 (variableName.Equals(Wildcard) || statement["condition"]["left"]["name"].Matches(variableName)
                                                || statement["condition"]["right"]["name"].Matches(variableName)))
        {
            var ifElseCount = 0;

            yield return new((contractPosition, functionPosition, statementPosition, null, ifElseCount), functionName);

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

        // 6.   If the statement is an expression statement such as 'test = var1 + var2'
        //      and either we are searching for a wildcard or one of the variables on the
        //      right matches the variable name
        else if (statement["type"].Matches(ExpressionStatement) && statement["expression"]["type"].Matches(BinaryOperation) &&
                 (variableName.Equals(Wildcard) ||
                  statement["expression"]["right"]["type"].Matches(Identifier) && statement["expression"]["right"]["name"].Matches(variableName) ||
                  statement["expression"]["right"]["type"].Matches(BinaryOperation) && (statement["expression"]["right"]["left"]["name"].Matches(variableName) ||
                      statement["expression"]["right"]["right"]["name"].Matches(variableName))))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 7.   If the statement is an expression statement and a function call such as 'storageA.store(numberB)'
        //      and either we are searching for a wildcard or one of the variables passed to the function call
        //      match the variable name
        else if (statement["type"].Matches(ExpressionStatement) && statement["expression"]["type"].Matches(FunctionCall))
        {
            var arguments = statement["expression"]["arguments"].ToSafeList();

            var argumentPosition = 0;
            foreach (var argument in arguments)
            {
                if (variableName.Equals(Wildcard) || argument["name"].Matches(variableName))
                {
                    yield return new((contractPosition, functionPosition, statementPosition, null, argumentPosition), functionName);
                }

                argumentPosition++;
            }
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
        // 1.   If the statement is a simple return statement such as 'return variableName'
        //      and either we are searching for a wildcard or the variable name matches
        if (statement["type"].Matches(ReturnStatement) &&
            (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, statement["expression"]["name"], variableType)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 2.   If the statement is a tuple return statement such as 'return (var1, var2)'
        //      then we need to check all components within the tuple and if either we are
        //      either searching for a wildcard, or at least one of the variable name matches
        else if (statement["type"].Matches(ReturnStatement) && statement["expression"]["type"].Matches(TupleExpression))
        {
            var components = statement["expression"]["components"].ToSafeList();

            var componentPosition = 0;
            foreach (var component in components)
            {
                if (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, component["name"], variableType))
                {
                    yield return new((contractPosition, functionPosition, statementPosition, null, componentPosition), functionName);
                    break;
                }

                componentPosition++;
            }
        }

        // 3.   If the statement is a variable declaration through a binary operation such as
        //      'var test = var1 == var2' then we need to check if either we are looking for a
        //      wildcard or either the left side or the right side variables match the variable name
        else if (statement["type"].Matches(VariableDeclarationStatement) && statement["initialValue"]["type"].Matches(BinaryOperation)
                 && (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, statement["initialValue"]["left"]["name"], variableType)
                                                   || IsVariableTypeMatch(contractName, statement["initialValue"]["right"]["name"], variableType)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 4.   If the statement is a variable declaration such as 'var test = var1', then we
        //      either check if we are looking for a wildcard or that the variable name matches
        else if (statement["type"].Matches(VariableDeclarationStatement) &&
                 (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, statement["initialValue"]["name"], variableType)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 5.   If the statement is an if statement and either we are looking for a wildcard
        //      or the left or right side of the condition have a variable matching the variable name
        else if (statement["type"].Matches(IfStatement) && statement["condition"]["type"].Matches(BinaryOperation) &&
                 (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, statement["condition"]["left"]["name"], variableType)
                                                || IsVariableTypeMatch(contractName, statement["condition"]["right"]["name"], variableType)))
        {
            var ifElseCount = 0;

            yield return new((contractPosition, functionPosition, statementPosition, null, ifElseCount), functionName);

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

        // 6.   If the statement is an expression statement such as 'test = var1 + var2'
        //      and either we are searching for a wildcard or one of the variables on the
        //      right matches the variable name
        else if (statement["type"].Matches(ExpressionStatement) && statement["expression"]["type"].Matches(BinaryOperation) &&
                 (variableType.Equals(Wildcard) ||
                  statement["expression"]["right"]["type"].Matches(Identifier) && IsVariableTypeMatch(contractName, statement["expression"]["right"]["name"], variableType) ||
                  statement["expression"]["right"]["type"].Matches(BinaryOperation) && (IsVariableTypeMatch(contractName, statement["expression"]["right"]["left"]["name"], variableType) ||
                      IsVariableTypeMatch(contractName, statement["expression"]["right"]["right"]["name"], variableType))))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 7.   If the statement is an expression statement and a function call such as 'storageA.store(numberB)'
        //      and either we are searching for a wildcard or one of the variables passed to the function call
        //      match the variable name
        else if (statement["type"].Matches(ExpressionStatement) && statement["expression"]["type"].Matches(FunctionCall))
        {
            var arguments = statement["expression"]["arguments"].ToSafeList();

            var argumentPosition = 0;
            foreach (var argument in arguments)
            {
                if (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, argument["name"], variableType))
                {
                    yield return new((contractPosition, functionPosition, statementPosition, null, argumentPosition), functionName);
                }

                argumentPosition++;
            }
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
        // 1.   If the statement is a simple return statement such as 'return variableName'
        //      and either we are searching for a wildcard or the variable name matches
        if (statement["type"].Matches(ReturnStatement) &&
            (variableVisibility.Equals(Wildcard) || IsVariableVisibilityMatch(contractName, statement["expression"]["name"], variableVisibility)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 2.   If the statement is a tuple return statement such as 'return (var1, var2)'
        //      then we need to check all components within the tuple and if either we are
        //      either searching for a wildcard, or at least one of the variable name matches
        else if (statement["type"].Matches(ReturnStatement) && statement["expression"]["type"].Matches(TupleExpression))
        {
            var components = statement["expression"]["components"].ToSafeList();

            var componentPosition = 0;
            foreach (var component in components)
            {
                if (variableVisibility.Equals(Wildcard) || IsVariableVisibilityMatch(contractName, component["name"], variableVisibility))
                {
                    yield return new((contractPosition, functionPosition, statementPosition, null, componentPosition), functionName);
                    break;
                }

                componentPosition++;
            }
        }

        // 3.   If the statement is a variable declaration through a binary operation such as
        //      'var test = var1 == var2' then we need to check if either we are looking for a
        //      wildcard or either the left side or the right side variables match the variable name
        else if (statement["type"].Matches(VariableDeclarationStatement) && statement["initialValue"]["type"].Matches(BinaryOperation)
                 && (variableVisibility.Equals(Wildcard) || IsVariableVisibilityMatch(contractName, statement["initialValue"]["left"]["name"], variableVisibility)
                                                         || IsVariableVisibilityMatch(contractName, statement["initialValue"]["right"]["name"], variableVisibility)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 4.   If the statement is a variable declaration such as 'var test = var1', then we
        //      either check if we are looking for a wildcard or that the variable name matches
        else if (statement["type"].Matches(VariableDeclarationStatement) &&
                 (variableVisibility.Equals(Wildcard) || IsVariableVisibilityMatch(contractName, statement["initialValue"]["name"], variableVisibility)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 5.   If the statement is an if statement and either we are looking for a wildcard
        //      or the left or right side of the condition have a variable matching the variable name
        else if (statement["type"].Matches(IfStatement) && statement["condition"]["type"].Matches(BinaryOperation) &&
                 (variableVisibility.Equals(Wildcard) || IsVariableVisibilityMatch(contractName, statement["condition"]["left"]["name"], variableVisibility)
                                                      || IsVariableVisibilityMatch(contractName, statement["condition"]["right"]["name"], variableVisibility)))
        {
            var ifElseCount = 0;

            yield return new((contractPosition, functionPosition, statementPosition, null, ifElseCount), functionName);

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

        // 6.   If the statement is an expression statement such as 'test = var1 + var2'
        //      and either we are searching for a wildcard or one of the variables on the
        //      right matches the variable name
        else if (statement["type"].Matches(ExpressionStatement) && statement["expression"]["type"].Matches(BinaryOperation) &&
                 (variableVisibility.Equals(Wildcard) ||
                  statement["expression"]["right"]["type"].Matches(Identifier) && IsVariableVisibilityMatch(contractName, statement["expression"]["right"]["name"], variableVisibility) ||
                  statement["expression"]["right"]["type"].Matches(BinaryOperation) && (IsVariableVisibilityMatch(contractName, statement["expression"]["right"]["left"]["name"], variableVisibility) ||
                      IsVariableVisibilityMatch(contractName, statement["expression"]["right"]["right"]["name"], variableVisibility))))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 7.   If the statement is an expression statement and a function call such as 'storageA.store(numberB)'
        //      and either we are searching for a wildcard or one of the variables passed to the function call
        //      match the variable name
        else if (statement["type"].Matches(ExpressionStatement) && statement["expression"]["type"].Matches(FunctionCall))
        {
            var arguments = statement["expression"]["arguments"].ToSafeList();

            var argumentPosition = 0;
            foreach (var argument in arguments)
            {
                if (variableVisibility.Equals(Wildcard) || IsVariableVisibilityMatch(contractName, argument["name"], variableVisibility))
                {
                    yield return new((contractPosition, functionPosition, statementPosition, null, argumentPosition), functionName);
                }

                argumentPosition++;
            }
        }
    }
}