using AspectSol.Lib.Infra.Extensions;
using Newtonsoft.Json.Linq;

namespace AspectSol.Lib.Domain.Filtering.Solidity;

public class VariableGettersFiltering : VariableFiltering
{
    protected override IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex, int? ArgumentIndex), string>>
        CheckStatementsForVariableName(List<JToken> statements, string variableName, int contractPosition, string functionName, int functionPosition)
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

    protected override IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex, int? ArgumentIndex), string>>
        CheckStatementForVariableName(JToken statement, string variableName, int contractPosition, string functionName, int functionPosition, int statementPosition)
    {
        // 1.   If the statement is a variable declaration such as 'var test = var1', then we either check if we are looking for a wildcard or that the
        //      variable name matches
        if (statement["nodeType"].Matches(VariableDeclarationStatement) &&
            (variableName.Equals(Wildcard) || statement["initialValue"]["name"].Matches(variableName)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 2.   If the statement is a variable declaration through a binary operation such as 'var test = var1 == var2' then we need to check if either we are
        //      looking for a wildcard or either the left side or the right side variables match the variable name
        else if (statement["nodeType"].Matches(VariableDeclarationStatement) && statement["initialValue"]["nodeType"].Matches(BinaryOperation) &&
                 (variableName.Equals(Wildcard) || statement["initialValue"]["leftExpression"]["name"].Matches(variableName) ||
                     statement["initialValue"]["rightExpression"]["name"].Matches(variableName)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 3.   If the statement is an expression statement such as 'test = var1', then we either check if we are looking for a wildcard or that the variable
        //      name matches
        else if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(Assignment) &&
                 (variableName.Equals(Wildcard) || statement["expression"]["rightHandSide"]["name"].Matches(variableName)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 4.   If the statement is an expression statement through an assignment such as 'test = var1 == var2' then we need to check if either we are looking
        //      for a wildcard or either the left side or the right side variables match the variable name
        else if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(Assignment) &&
                 statement["expression"]["rightHandSide"]["nodeType"].Matches(BinaryOperation) && (variableName.Equals(Wildcard) ||
                     statement["expression"]["rightHandSide"]["rightExpression"]["name"].Matches(variableName) ||
                     statement["expression"]["rightHandSide"]["leftExpression"]["name"].Matches(variableName)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 5.   If the statement is an expression statement and a function call such as 'storageA.store(numberB)' and either we are searching for a wildcard or
        //      one of the variables passed to the function call match the variable name
        else if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(FunctionCall))
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

        // 6.   If the statement is a simple return statement such as 'return variableName' and either we are searching for a wildcard or the variable
        //      name matches
        else if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(Identifier) &&
                 (variableName.Equals(Wildcard) || statement["expression"]["name"].Matches(variableName)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 7.   If the statement is a tuple return statement such as 'return (var1, var2)' then we need to check all components within the tuple and if either
        //      we are either searching for a wildcard, or at least one of the variable name matches
        else if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(TupleExpression))
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

        // 8.   If the statement is a return statement through a binary operation such as 'return var1 + var2' then we need to check all components within the
        //      tuple and if either we are either searching for a wildcard, or at least one of the variable name matches
        else if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(BinaryOperation) && (variableName.Equals(Wildcard) ||
                     statement["expression"]["leftExpression"]["name"].Matches(variableName) ||
                     statement["expression"]["rightExpression"]["name"].Matches(variableName)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 9.   If the statement is an if statement and either we are looking for a wildcard or the left or right side of the condition have a variable
        //      matching the variable name
        else if (statement["nodeType"].Matches(IfStatement) && statement["condition"]["nodeType"].Matches(BinaryOperation) && (variableName.Equals(Wildcard) ||
                     statement["condition"]["leftExpression"]["name"].Matches(variableName) ||
                     statement["condition"]["rightExpression"]["name"].Matches(variableName)))
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

            if (!statement["falseBody"].IsValueNullOrWhitespace() && statement["falseBody"]["nodeType"].Matches("Block"))
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
            else if (!statement["falseBody"].IsValueNullOrWhitespace())
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
    }

    protected override IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex, int? ArgumentIndex), string>>
        CheckStatementsForVariableType(List<JToken> statements, string variableType, string contractName, int contractPosition, string functionName, int functionPosition)
    {
        var statementPosition = 0;
        foreach (var statement in statements)
        {
            foreach (var interestedStatement in CheckStatementForVariableType(statement, variableType, contractName, contractPosition, functionName,
                         functionPosition,
                         statementPosition))
            {
                yield return interestedStatement;
            }

            statementPosition++;
        }
    }

    protected override IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex, int? ArgumentIndex), string>>
        CheckStatementForVariableType(JToken statement, string variableType, string contractName, int contractPosition, string functionName,
            int functionPosition, int statementPosition)
    {
        // 1.   If the statement is a variable declaration such as 'var test = var1', then we either check if we are looking for a wildcard or that the
        //      variable name matches
        if (statement["nodeType"].Matches(VariableDeclarationStatement) &&
            (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, statement["initialValue"]["name"], variableType)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 2.   If the statement is a variable declaration through a binary operation such as 'var test = var1 == var2' then we need to check if either we are
        //      looking for a wildcard or either the left side or the right side variables match the variable name
        else if (statement["nodeType"].Matches(VariableDeclarationStatement) && statement["initialValue"]["nodeType"].Matches(BinaryOperation) &&
                 (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, statement["initialValue"]["leftExpression"]["name"], variableType) ||
                     IsVariableTypeMatch(contractName, statement["initialValue"]["rightExpression"]["name"], variableType)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 3.   If the statement is an expression statement such as 'test = var1', then we either check if we are looking for a wildcard or that the variable
        //      name matches
        else if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(Assignment) &&
                 (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, statement["expression"]["rightHandSide"]["name"], variableType)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 4.   If the statement is an expression statement through an assignment such as 'test = var1 == var2' then we need to check if either we are looking
        //      for a wildcard or either the left side or the right side variables match the variable name
        else if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(Assignment) &&
                 statement["expression"]["rightHandSide"]["nodeType"].Matches(BinaryOperation) && (variableType.Equals(Wildcard) ||
                     IsVariableTypeMatch(contractName, statement["expression"]["rightHandSide"]["rightExpression"]["name"], variableType) ||
                     IsVariableTypeMatch(contractName, statement["expression"]["rightHandSide"]["leftExpression"]["name"], variableType)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 5.   If the statement is an expression statement and a function call such as 'storageA.store(numberB)' and either we are searching for a wildcard or
        //      one of the variables passed to the function call match the variable name
        else if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(FunctionCall))
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

        // 6.   If the statement is a simple return statement such as 'return variableName' and either we are searching for a wildcard or the variable
        //      name matches
        else if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(Identifier) &&
                 (variableType.Equals(Wildcard) || IsVariableTypeMatch(contractName, statement["expression"]["name"], variableType)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 7.   If the statement is a tuple return statement such as 'return (var1, var2)' then we need to check all components within the tuple and if either
        //      we are either searching for a wildcard, or at least one of the variable name matches
        else if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(TupleExpression))
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

        // 8.   If the statement is a return statement through a binary operation such as 'return var1 + var2' then we need to check all components within the
        //      tuple and if either we are either searching for a wildcard, or at least one of the variable name matches
        else if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(BinaryOperation) && (variableType.Equals(Wildcard) ||
                     IsVariableTypeMatch(contractName, statement["expression"]["leftExpression"]["name"], variableType) ||
                     IsVariableTypeMatch(contractName, statement["expression"]["rightExpression"]["name"], variableType)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 9.   If the statement is an if statement and either we are looking for a wildcard or the left or right side of the condition have a variable
        //      matching the variable name
        else if (statement["nodeType"].Matches(IfStatement) && statement["condition"]["nodeType"].Matches(BinaryOperation) && (variableType.Equals(Wildcard) ||
                     IsVariableTypeMatch(contractName, statement["condition"]["leftExpression"]["name"], variableType) ||
                     IsVariableTypeMatch(contractName, statement["condition"]["rightExpression"]["name"], variableType)))
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

            if (!statement["falseBody"].IsValueNullOrWhitespace() && statement["falseBody"]["nodeType"].Matches("Block"))
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
            else if (!statement["falseBody"].IsValueNullOrWhitespace())
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
    }

    protected override IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex, int? ArgumentIndex), string>>
        CheckStatementsForVariableVisibility(List<JToken> statements, string variableVisibility, string contractName, int contractPosition, string functionName, int functionPosition)
    {
        var statementPosition = 0;
        foreach (var statement in statements)
        {
            foreach (var interestedStatement in CheckStatementForVariableVisibility(statement, variableVisibility, contractName, contractPosition, functionName,
                         functionPosition,
                         statementPosition))
            {
                yield return interestedStatement;
            }

            statementPosition++;
        }
    }

    protected override IEnumerable<KeyValuePair<(int ContractIndex, int FunctionIndex, int StatementIndex, int? SubStatementIndex, int? ArgumentIndex), string>>
        CheckStatementForVariableVisibility(JToken statement, string variableVisibility, string contractName, int contractPosition, string functionName, int functionPosition,
            int statementPosition)
    {
        // 1.   If the statement is a variable declaration such as 'var test = var1', then we either check if we are looking for a wildcard or that the
        //      variable name matches
        if (statement["nodeType"].Matches(VariableDeclarationStatement) &&
            (variableVisibility.Equals(Wildcard) || IsVariableTypeMatch(contractName, statement["initialValue"]["name"], variableVisibility)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 2.   If the statement is a variable declaration through a binary operation such as 'var test = var1 == var2' then we need to check if either we are
        //      looking for a wildcard or either the left side or the right side variables match the variable name
        else if (statement["nodeType"].Matches(VariableDeclarationStatement) && statement["initialValue"]["nodeType"].Matches(BinaryOperation) &&
                 (variableVisibility.Equals(Wildcard) ||
                     IsVariableVisibilityMatch(contractName, statement["initialValue"]["leftExpression"]["name"], variableVisibility) ||
                     IsVariableVisibilityMatch(contractName, statement["initialValue"]["rightExpression"]["name"], variableVisibility)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 3.   If the statement is an expression statement such as 'test = var1', then we either check if we are looking for a wildcard or that the variable
        //      name matches
        else if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(Assignment) &&
                 (variableVisibility.Equals(Wildcard) ||
                     IsVariableVisibilityMatch(contractName, statement["expression"]["rightHandSide"]["name"], variableVisibility)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 4.   If the statement is an expression statement through an assignment such as 'test = var1 == var2' then we need to check if either we are looking
        //      for a wildcard or either the left side or the right side variables match the variable name
        else if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(Assignment) &&
                 statement["expression"]["rightHandSide"]["nodeType"].Matches(BinaryOperation) && (variableVisibility.Equals(Wildcard) ||
                     IsVariableVisibilityMatch(contractName, statement["expression"]["rightHandSide"]["rightExpression"]["name"], variableVisibility) ||
                     IsVariableVisibilityMatch(contractName, statement["expression"]["rightHandSide"]["leftExpression"]["name"], variableVisibility)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 5.   If the statement is an expression statement and a function call such as 'storageA.store(numberB)' and either we are searching for a wildcard or
        //      one of the variables passed to the function call match the variable name
        else if (statement["nodeType"].Matches(ExpressionStatement) && statement["expression"]["nodeType"].Matches(FunctionCall))
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

        // 6.   If the statement is a simple return statement such as 'return variableName' and either we are searching for a wildcard or the variable
        //      name matches
        else if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(Identifier) &&
                 (variableVisibility.Equals(Wildcard) || IsVariableVisibilityMatch(contractName, statement["expression"]["name"], variableVisibility)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 7.   If the statement is a tuple return statement such as 'return (var1, var2)' then we need to check all components within the tuple and if either
        //      we are either searching for a wildcard, or at least one of the variable name matches
        else if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(TupleExpression))
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

        // 8.   If the statement is a return statement through a binary operation such as 'return var1 + var2' then we need to check all components within the
        //      tuple and if either we are either searching for a wildcard, or at least one of the variable name matches
        else if (statement["nodeType"].Matches(Return) && statement["expression"]["nodeType"].Matches(BinaryOperation) && (variableVisibility.Equals(Wildcard) ||
                     IsVariableVisibilityMatch(contractName, statement["expression"]["leftExpression"]["name"], variableVisibility) ||
                     IsVariableVisibilityMatch(contractName, statement["expression"]["rightExpression"]["name"], variableVisibility)))
        {
            yield return new((contractPosition, functionPosition, statementPosition, null, null), functionName);
        }

        // 9.   If the statement is an if statement and either we are looking for a wildcard or the left or right side of the condition have a variable
        //      matching the variable name
        else if (statement["nodeType"].Matches(IfStatement) && statement["condition"]["nodeType"].Matches(BinaryOperation) && (variableVisibility.Equals(Wildcard) ||
                     IsVariableVisibilityMatch(contractName, statement["condition"]["leftExpression"]["name"], variableVisibility) ||
                     IsVariableVisibilityMatch(contractName, statement["condition"]["rightExpression"]["name"], variableVisibility)))
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

            if (!statement["falseBody"].IsValueNullOrWhitespace() && statement["falseBody"]["nodeType"].Matches("Block"))
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
            else if (!statement["falseBody"].IsValueNullOrWhitespace())
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
    }
}