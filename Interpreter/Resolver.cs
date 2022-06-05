using Interpreter.Expressions;
using Interpreter.Statements;
using Interpreter.Utils;

namespace Interpreter
{
    internal static class Resolver
    {
        private static readonly Stack<Dictionary<string, bool>> scopes = new();

        public static void ResolveStatement(IStatement statement)
        {
            if (statement is BlockStatement blockStatement)
            {
                ResolveStatement(blockStatement);
                return;
            }
            if (statement is DeclarationStatement declarationStatement)
            {
                ResolveStatement(declarationStatement);
                return;
            }
            if (statement is FunctionStatement functionStatement)
            {
                ResolveStatement(functionStatement);
                return;
            }
            if (statement is ExpressionStatement expressionStatement)
            {
                ResolveStatement(expressionStatement);
                return;
            }
            if (statement is IfStatement ifStatement)
            {
                ResolveStatement(ifStatement);
                return;
            }
            if (statement is PrintStatement printStatement)
            {
                ResolveStatement(printStatement);
                return;
            }
            if (statement is ReturnStatement returnStatement)
            {
                ResolveStatement(returnStatement);
                return;
            }
            if (statement is WhileStatement whileStatement)
            {
                ResolveStatement(whileStatement);
                return;
            }
        }

        public static void ResolveExpression(IExpression expression)
        {
            if (expression is VariableExpression variableExpression)
            {
                ResolveExpression(variableExpression);
                return;
            }
            if (expression is AssignmentExpression assignmentExpression)
            {
                ResolveExpression(assignmentExpression);
                return;
            }
            if (expression is BinaryExpression binaryExpression)
            {
                ResolveExpression(binaryExpression);
                return;
            }
            if (expression is CallExpression callExpression)
            {
                ResolveExpression(callExpression);
                return;
            }
            if (expression is GroupingExpression groupingExpression)
            {
                ResolveExpression(groupingExpression);
                return;
            }
            if (expression is LiteralExpression literalExpression)
            {
                ResolveExpression(literalExpression);
                return;
            }
            if (expression is LogicalExpression logicalExpression)
            {
                ResolveExpression(logicalExpression);
                return;
            }
            if (expression is UnaryExpression unaryExpression)
            {
                ResolveExpression(unaryExpression);
                return;
            }
        }

        private static void ResolveStatement(BlockStatement blockStatement)
        {
            BeginScope();
            ResolveStatements(blockStatement.Statements);
            EndScope();
        }

        private static void ResolveStatement(DeclarationStatement declarationStatement)
        {
            Declare(declarationStatement.Name);
            if (declarationStatement.Expression is not null)
            {
                ResolveExpression(declarationStatement.Expression);
            }
            Define(declarationStatement.Name);
        }

        private static void ResolveStatement(FunctionStatement functionStatement)
        {
            Declare(functionStatement.Name);
            Define(functionStatement.Name);
            BeginScope();
            foreach (Token? token in functionStatement.Parameters)
            {
                if (token is not null)
                {
                    Define(token);
                    Declare(token);
                }
            }
            ResolveStatements(functionStatement.Body);
            EndScope();
        }

        private static void ResolveStatement(ExpressionStatement expressionStatement)
        {
            ResolveExpression(expressionStatement.Expression);
        }

        private static void ResolveStatement(IfStatement ifStatement)
        {
            ResolveExpression(ifStatement.Condition);
            ResolveStatement(ifStatement.ThenBranch);
            if (ifStatement.ElseBranch is not null)
            {
                ResolveStatement(ifStatement.ElseBranch);
            }
        }

        private static void ResolveStatement(PrintStatement printStatement)
        {
            ResolveExpression(printStatement.Expression);
        }

        private static void ResolveStatement(ReturnStatement returnStatement)
        {
            if (returnStatement.Expression is not null)
            {
                ResolveExpression(returnStatement.Expression);
            }
        }

        private static void ResolveStatement(WhileStatement whileStatement)
        {
            ResolveExpression(whileStatement.Condition);
            ResolveStatement(whileStatement.Body);
        }

        private static void ResolveExpression(VariableExpression variableExpression)
        {
            if (scopes.Count is not 0 && !scopes.Peek()[variableExpression.Token.Lexeme])
            {
                ErrorUtils.Error(variableExpression.Token, "Can't read local variable in it's own initializer");
            }
            ResolveLocal(variableExpression, variableExpression.Token);
        }

        private static void ResolveExpression(AssignmentExpression assignmentExpression)
        {
            ResolveExpression(assignmentExpression.Value);
            ResolveLocal(assignmentExpression, assignmentExpression.Token);
        }

        private static void ResolveExpression(BinaryExpression binaryExpression)
        {
            ResolveExpression(binaryExpression.Left);
            ResolveExpression(binaryExpression.Right);
        }

        private static void ResolveExpression(CallExpression callExpression)
        {
            ResolveExpression(callExpression.Calle);
            foreach (IExpression? argument in callExpression.Arguments)
            {
                if (argument is not null)
                {
                    ResolveExpression(argument);
                }
            }
        }

        private static void ResolveExpression(GroupingExpression groupingExpression)
        {
            ResolveExpression(groupingExpression.Expression);
        }

        private static void ResolveExpression(LiteralExpression literalExpression)
        {
            return;
        }

        private static void ResolveExpression(LogicalExpression logicalExpression)
        {
            ResolveExpression(logicalExpression.Left);
            ResolveExpression(logicalExpression.Right);
        }

        private static void ResolveExpression(UnaryExpression unaryExpression)
        {
            ResolveExpression(unaryExpression.Right);
        }

        private static void ResolveLocal(IExpression expression, Token token)
        {
            int i = scopes.Count - 1;
            foreach (Dictionary<string, bool>? scope in scopes.Reverse())
            {
                if (scope.ContainsKey(token.Lexeme))
                {
                    LoxInterpreter.Resolve(expression, scopes.Count - 1 - i);
                }
                i--;
            }
        }

        private static void ResolveStatements(IReadOnlyList<IStatement> statements)
        {
            foreach (IStatement statement in statements)
            {
                ResolveStatement(statement);
            }
        }

        private static void Declare(Token identifierToken)
        {
            if (scopes.Count is 0)
            {
                return;
            }
            Dictionary<string, bool> scope = scopes.Peek();
            scope.Add(identifierToken.Lexeme, false);
        }

        private static void Define(Token identifierToken)
        {
            if (scopes.Count is 0)
            {
                return;
            }
            scopes.Peek().Add(identifierToken.Lexeme, true);
        }

        private static void BeginScope()
        {
            scopes.Push(new Dictionary<string, bool>());
        }

        private static void EndScope()
        {
            scopes.Pop();
        }
    }
}
