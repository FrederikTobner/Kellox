﻿using Interpreter.Expressions;
using Interpreter.Statements;
using Interpreter.Utils;

namespace Interpreter
{
    /// <summary>
    /// Walks over the Syntaxtree before it is executed and resolves all the variables it contains
    /// </summary>
    internal static class Resolver
    {
        /// <summary>
        /// Stack for keeping track of the variables in the current scope and all outer scopes
        /// </summary>
        private static readonly Stack<Dictionary<string, bool>> scopes = new();

        /// <summary>
        /// Resolves a LoxProgram -> walks over the Syntaxtree and resolves all the variables it contains
        /// </summary>
        /// <param name="program">The LoxProgram that is getting resolved</param>
        public static void Resolve(LoxProgram program)
        {
            if (!program.Runnable)
            {
                return;
            }
            ResolveStatements(program.Statements);
        }


        /// <summary>
        ///  Casts the Statement to it's respective type and handles further resolving
        /// </summary>
        private static void ResolveStatement(IStatement statement)
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

        /// <summary>
        ///  Casts the Expression to it's respective type and handles further resolving
        /// </summary>
        private static void ResolveExpression(IExpression expression)
        {
            if (expression is LiteralExpression)
            {
                return;
            }
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

        /// <summary>
        /// Resolves a BlockStatement
        /// </summary>
        /// <param name="blockStatement">The BlockStatement that is resolved</param>
        private static void ResolveStatement(BlockStatement blockStatement)
        {
            BeginScope();
            ResolveStatements(blockStatement.Statements);
            EndScope();
        }


        /// <summary>
        /// Resolves a DeclarationStatement
        /// </summary>
        /// <param name="declarationStatement">The Declaration that is resolved</param>
        private static void ResolveStatement(DeclarationStatement declarationStatement)
        {
            Declare(declarationStatement.Name);
            if (declarationStatement.Expression is not null)
            {
                ResolveExpression(declarationStatement.Expression);
                Define(declarationStatement.Name);
            }

        }

        /// <summary>
        /// Resolves a FunctionStatement
        /// </summary>
        /// <param name="functionStatement">The FunctionStatement that shall be resolved</param>
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

        /// <summary>
        /// Resolves a ExpressionStatement
        /// </summary>
        /// <param name="expressionStatement">The ExpressionStatement that shall be resolved</param>
        private static void ResolveStatement(ExpressionStatement expressionStatement)
        {
            ResolveExpression(expressionStatement.Expression);
        }

        /// <summary>
        /// Resolves a IfStatement
        /// </summary>
        /// <param name="ifStatement">The IfStatement that shall be resolved</param>
        private static void ResolveStatement(IfStatement ifStatement)
        {
            ResolveExpression(ifStatement.Condition);
            ResolveStatement(ifStatement.ThenBranch);
            if (ifStatement.ElseBranch is not null)
            {
                ResolveStatement(ifStatement.ElseBranch);
            }
        }

        /// <summary>
        /// Resolves a PrintStatement
        /// </summary>
        /// <param name="printStatement">The PrintStatement that shall be resolved</param>
        private static void ResolveStatement(PrintStatement printStatement)
        {
            ResolveExpression(printStatement.Expression);
        }

        /// <summary>
        /// Resolves a ReturnStatement
        /// </summary>
        /// <param name="returnStatement">The ReturnStatement that shall be resolved</param>
        private static void ResolveStatement(ReturnStatement returnStatement)
        {
            if (returnStatement.Expression is not null)
            {
                ResolveExpression(returnStatement.Expression);
            }
        }

        /// <summary>
        /// Resolves a WhileStatement
        /// </summary>
        /// <param name="whileStatement">The ReturnStatement that shall be resolved</param>
        private static void ResolveStatement(WhileStatement whileStatement)
        {
            ResolveExpression(whileStatement.Condition);
            ResolveStatement(whileStatement.Body);
        }

        /// <summary>
        /// Resolves a VariableExpression
        /// </summary>
        /// <param name="variableExpression">The VariableExpression that shall be resolved</param>
        private static void ResolveExpression(VariableExpression variableExpression)
        {
            if (scopes.Count is not 0 && scopes.Peek().ContainsKey(variableExpression.Token.Lexeme) && !scopes.Peek()[variableExpression.Token.Lexeme])
            {
                ErrorUtils.Error(variableExpression.Token, "Can't read local variable in it's own initializer");
            }
            ResolveLocal(variableExpression, variableExpression.Token);
        }

        /// <summary>
        /// Resolves a AssignmentExpression
        /// </summary>
        /// <param name="assignmentExpression">The AssignmentExpression that shall be resolved</param>
        private static void ResolveExpression(AssignmentExpression assignmentExpression)
        {
            ResolveExpression(assignmentExpression.Value);
            ResolveLocal(assignmentExpression, assignmentExpression.Token);
        }

        /// <summary>
        /// Resolves a BinaryExpression
        /// </summary>
        /// <param name="binaryExpression">The BinaryExpression that shall be resolved</param>
        private static void ResolveExpression(BinaryExpression binaryExpression)
        {
            ResolveExpression(binaryExpression.Left);
            ResolveExpression(binaryExpression.Right);
        }

        /// <summary>
        /// Resolves a CallExpression
        /// </summary>
        /// <param name="callExpression">The CallExpression that shall be resolved</param>
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

        /// <summary>
        /// Resolves a GroupingExpression
        /// </summary>
        /// <param name="groupingExpression">The GroupingExpression that shall be resolved</param>
        private static void ResolveExpression(GroupingExpression groupingExpression)
        {
            ResolveExpression(groupingExpression.Expression);
        }

        /// <summary>
        /// Resolves a LogicalExpression
        /// </summary>
        /// <param name="logicalExpression">The LogicalExpression that shall be resolved</param>
        private static void ResolveExpression(LogicalExpression logicalExpression)
        {
            ResolveExpression(logicalExpression.Left);
            ResolveExpression(logicalExpression.Right);
        }

        /// <summary>
        /// Resolves a UnaryExpression
        /// </summary>
        /// <param name="unaryExpression">The UnaryExpression that shall be resolved</param>
        private static void ResolveExpression(UnaryExpression unaryExpression)
        {
            ResolveExpression(unaryExpression.Right);
        }

        /// <summary>
        /// Resolves a VariableExpression or AssignmentExpression
        /// </summary>
        /// <param name="expression">The expression containing the variable</param>
        /// <param name="token">The Token of the variable</param>
        private static void ResolveLocal(IExpression expression, Token token)
        {
            int i = scopes.Count - 1;
            foreach (Dictionary<string, bool>? scope in scopes.Reverse())
            {
                if (scope.ContainsKey(token.Lexeme))
                {
                    LoxInterpreter.Resolve(expression, scopes.Count - i);
                }
                i--;
            }
        }

        /// <summary>
        /// Resolves a read-only List of Statements
        /// </summary>
        /// <param name="statements">The list that shall be resolved</param>
        private static void ResolveStatements(IReadOnlyList<IStatement> statements)
        {
            foreach (IStatement statement in statements)
            {
                ResolveStatement(statement);
            }
        }

        /// <summary>
        /// Declares a Token
        /// </summary>
        /// <param name="identifierToken">The Token of the identifier</param>
        private static void Declare(Token identifierToken)
        {
            if (scopes.Count is 0)
            {
                return;
            }
            Dictionary<string, bool> scope = scopes.Peek();
            scope.Add(identifierToken.Lexeme, false);
        }

        /// <summary>
        /// Defines a single Token
        /// </summary>
        /// <param name="identifierToken"></param>
        private static void Define(Token identifierToken)
        {
            if (scopes.Count is 0)
            {
                return;
            }
            if (scopes.Peek().ContainsKey(identifierToken.Lexeme))
            {
                scopes.Peek()[identifierToken.Lexeme] = true;
                return;
            }
            scopes.Peek().Add(identifierToken.Lexeme, true);
        }

        /// <summary>
        /// Begins a new scope
        /// </summary>
        private static void BeginScope()
        {
            scopes.Push(new Dictionary<string, bool>());
        }

        /// <summary>
        /// Ends the current scope
        /// </summary>
        private static void EndScope()
        {
            scopes.Pop();
        }
    }
}