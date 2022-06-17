using Lox.Expressions;
using Lox.Interpreter;
using Lox.Statements;
using Lox.Tokens;
using Lox.Utils;

namespace Lox.Resolver;

/// <summary>
/// Walks over the Syntaxtree before it is executed and resolves all the variables it contains
/// </summary>
internal static class LoxResolver
{
    /// <summary>
    /// Stack for keeping track of the variables in the current scope and all outer scopes.
    /// Each scope is defined as a dictionary in the stack
    /// </summary>
    private static readonly Stack<Dictionary<string, bool>> scopes = new();

    /// <summary>
    /// The current function type -> used to keep track if the resolver is currently in a function
    /// </summary>
    private static FunctionType currentFunction = FunctionType.NONE;

    /// <summary>
    /// The current class type -> used to keep track if the resolver is currently in a class
    /// </summary>
    private static ClassType currentClass = ClassType.NONE;

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

    #region Statements

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
        if (statement is ClassStatement classStatement)
        {
            ResolveStatement(classStatement);
            return;
        }
        if (statement is FunctionStatement functionStatement)
        {
            ResolveStatement(functionStatement, FunctionType.FUNCTION);
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
            //A Literal can be ignored
            return;
        }
        if (expression is GetExpression getExpression)
        {
            ResolveExpression(getExpression);
            return;
        }
        if (expression is SuperExpression superExpression)
        {
            ResolveExpression(superExpression);
            return;
        }
        if (expression is ThisExpression thisExpression)
        {
            ResolveExpression(thisExpression);
            return;
        }
        if (expression is SetExpression setExpression)
        {
            ResolveExpression(setExpression);
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
        if (scopes.Count is not 0 && scopes.Peek().ContainsKey(declarationStatement.Name.Lexeme))
        {
            LoxErrorLogger.Error(declarationStatement.Name, "Variable with this name already defined in scope");
        }
        Declare(declarationStatement.Name);
        if (declarationStatement.Expression is not null)
        {
            ResolveExpression(declarationStatement.Expression);
            Define(declarationStatement.Name);
        }
    }

    /// <summary>
    /// Resolves a classStatement
    /// </summary>
    /// <param name="classStatement"></param>
    private static void ResolveStatement(ClassStatement classStatement)
    {
        ClassType enclosingClass = currentClass;
        currentClass = ClassType.CLASS;
        Define(classStatement.Token);
        if (classStatement.SuperClass is not null)
        {
            if (classStatement.SuperClass.Token.Lexeme.Equals(classStatement.Token.Lexeme))
            {
                LoxErrorLogger.Error(classStatement.Token, "A class can't inherit from itself");
            }
            else
            {
                currentClass = ClassType.SUBCLASS;
                ResolveExpression(classStatement.SuperClass);
            }
        }
        if (classStatement.SuperClass is not null)
        {
            BeginScope();
            scopes.Peek().Add("super", true);
        }
        BeginScope();
        foreach (FunctionStatement? method in classStatement.Methods)
        {
            if (method.Token.Lexeme.Equals("init"))
            {
                ResolveStatement(method, FunctionType.INITIALIZER);
            }
            else
            {
                ResolveStatement(method, FunctionType.METHOD);
            }

        }
        EndScope();
        if (classStatement.SuperClass is not null)
        {
            EndScope();
        }
        currentClass = enclosingClass;
    }

    /// <summary>
    /// Resolves a FunctionStatement
    /// </summary>
    /// <param name="functionStatement">The FunctionStatement that shall be resolved</param>
    private static void ResolveStatement(FunctionStatement functionStatement, FunctionType functionType)
    {
        FunctionType enclosingType = currentFunction;
        currentFunction = functionType;
        Define(functionStatement.Token);
        BeginScope();
        foreach (Token? token in functionStatement.Parameters)
        {
            if (token is not null)
            {
                Define(token);
            }
        }
        ResolveStatements(functionStatement.Body);
        EndScope();
        currentFunction = enclosingType;
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
        if (currentFunction is FunctionType.NONE)
        {
            LoxErrorLogger.Error(returnStatement.Keyword, "Can't return from top level code");
        }
        if (returnStatement.Expression is not null)
        {
            if (currentFunction is FunctionType.INITIALIZER)
            {
                LoxErrorLogger.Error(returnStatement.Keyword, "Can't return from an initializer");
            }
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

    #endregion

    #region Expressions
    /// <summary>
    /// Resolves a super expression
    /// </summary>
    /// <param name="superExpression"></param>
    private static void ResolveExpression(SuperExpression superExpression)
    {
        if (currentClass is ClassType.NONE)
        {
            LoxErrorLogger.Error(superExpression.Token, "Can't use \'super\' outside of a class");
        }
        else if (currentClass is not ClassType.SUBCLASS)
        {
            LoxErrorLogger.Error(superExpression.Token, "Can't use \'super\' with no superclass");
        }
        ResolveLocal(superExpression, superExpression.Token);
    }

    /// <summary>
    /// Resolves a ThisExpression
    /// </summary>
    /// <param name="thisExpression">The ThisExpression that shall be resolved</param>
    private static void ResolveExpression(ThisExpression thisExpression)
    {
        if (currentClass is ClassType.NONE)
        {
            LoxErrorLogger.Error(thisExpression.Token, "Cant use \'this\' outside of a class");
            return;
        }
        ResolveLocal(thisExpression, thisExpression.Token);
    }

    /// <summary>
    /// Resolves a GetExpression
    /// </summary>
    /// <param name="getExpression">The GetExpression that shall be resolved</param>
    private static void ResolveExpression(GetExpression getExpression)
    {
        ResolveExpression(getExpression.Object);
    }

    /// <summary>
    /// Resolves a SetExpression
    /// </summary>
    /// <param name="setExpression">The GetExpression that shall be resolved</param>
    private static void ResolveExpression(SetExpression setExpression)
    {
        ResolveExpression(setExpression.Value);
        ResolveExpression(setExpression.Object);
    }

    /// <summary>
    /// Resolves a VariableExpression
    /// </summary>
    /// <param name="variableExpression">The VariableExpression that shall be resolved</param>
    private static void ResolveExpression(VariableExpression variableExpression)
    {
        if (scopes.Count is not 0 && scopes.Peek().ContainsKey(variableExpression.Token.Lexeme) && !scopes.Peek()[variableExpression.Token.Lexeme])
        {
            LoxErrorLogger.Error(variableExpression.Token, "Can't read local variable in it's own initializer");
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
        foreach (Dictionary<string, bool>? scope in scopes)
        {
            if (scope.ContainsKey(token.Lexeme))
            {
                LoxInterpreter.Resolve(expression, scopes.Count - 1 - i);
            }
            i--;
        }
    }

    #endregion

    #region DeclaringAndDefining

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
        if (scope.ContainsKey(identifierToken.Lexeme))
        {
            LoxErrorLogger.Error(identifierToken.Line, "A variable with this name (" + identifierToken.Lexeme + ") was already defined in this scope");
            return;
        }
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
    #endregion

    #region ScopeHandling

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

    #endregion
}
