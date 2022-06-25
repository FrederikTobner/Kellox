using Kellox.Expressions;
using Kellox.Interpreter;
using Kellox.Keywords;
using Kellox.Statements;
using Kellox.Tokens;
using Kellox.Utils;

namespace Kellox.Resolver;

/// <summary>
/// Walks over the Syntaxtree before it is executed and resolves all the variables it contains
/// </summary>
internal static class KelloxResolver
{

    /// <summary>
    /// Stack for keeping track of the variables in the current scope and all outer scopes.
    /// Each scope is defined as a dictionary in the stack
    /// </summary>
    private static readonly Stack<Dictionary<string, bool>> scopes = new();

    /// <summary>
    /// The current function type -> used to keep track if the resolver is currently in a function
    /// </summary>
    private static FunctionType currentFunctionType = FunctionType.NONE;

    /// <summary>
    /// The current class type -> used to keep track if the resolver is currently in a class
    /// </summary>
    private static ClassType currentClassType = ClassType.NONE;

    /// <summary>
    /// Resolves a LoxProgram -> walks over the Syntaxtree and resolves all the variables it contains
    /// </summary>
    /// <param name="program">The LoxProgram that is getting resolved</param>
    public static void Resolve(KelloxProgram program)
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
        switch (statement)
        {
            case BlockStatement blockStatement:
                ResolveStatement(blockStatement);
                return;
            case DeclarationStatement declarationStatement:
                ResolveStatement(declarationStatement);
                return;
            case ClassStatement classStatement:
                ResolveStatement(classStatement);
                return;
            case FunctionStatement functionStatement:
                ResolveStatement(functionStatement, FunctionType.FUNCTION);
                return;
            case ExpressionStatement expressionStatement:
                ResolveStatement(expressionStatement);
                return;
            case IfStatement ifStatement:
                ResolveStatement(ifStatement);
                return;
            case PrintStatement printStatement:
                ResolveStatement(printStatement);
                return;
            case ReturnStatement returnStatement:
                ResolveStatement(returnStatement);
                return;
            case WhileStatement whileStatement:
                ResolveStatement(whileStatement);
                return;
            default:
                throw new NotImplementedException();
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
            ErrorLogger.Error(declarationStatement.Name, "A variable with this name (" + declarationStatement.Name.Lexeme + ") was already defined in this scope");
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
        ClassType enclosingClass = currentClassType;
        currentClassType = ClassType.CLASS;
        Define(classStatement.Token);
        if (classStatement.SuperClass is not null)
        {
            if (classStatement.SuperClass.Token.Lexeme.Equals(classStatement.Token.Lexeme))
            {
                ErrorLogger.Error(classStatement.Token, "A class can't inherit from itself");
                return;
            }
            currentClassType = ClassType.SUBCLASS;
            ResolveExpression(classStatement.SuperClass);
            BeginScope();
            scopes.Peek().Add(KeywordConstants.SuperKeyword, true);

        }
        BeginScope();
        foreach (FunctionStatement? method in classStatement.Methods)
        {
            ResolveStatement(method, method.Token.Lexeme.Equals(KeywordConstants.InitKeyword) ? FunctionType.INITIALIZER : FunctionType.METHOD);
        }
        EndScope();
        if (classStatement.SuperClass is not null)
        {
            EndScope();
        }
        currentClassType = enclosingClass;
    }

    /// <summary>
    /// Resolves a FunctionStatement
    /// </summary>
    /// <param name="functionStatement">The FunctionStatement that shall be resolved</param>
    private static void ResolveStatement(FunctionStatement functionStatement, FunctionType functionType)
    {
        FunctionType enclosingType = currentFunctionType;
        currentFunctionType = functionType;
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
        currentFunctionType = enclosingType;
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
        if (currentFunctionType is FunctionType.NONE)
        {
            ErrorLogger.Error(returnStatement.Keyword, "Can't return from top level code");
        }
        if (returnStatement.Expression is not null)
        {
            if (currentFunctionType is FunctionType.INITIALIZER)
            {
                ErrorLogger.Error(returnStatement.Keyword, "Can't return from an initializer");
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

    #endregion Statements

    #region Expressions

    /// <summary>
    /// Casts the Expression to it's respective type and handles further resolving
    /// </summary>
    private static void ResolveExpression(IExpression expression)
    {
        switch (expression)
        {
            case LiteralExpression:
                //A Literal can be ignored
                return;
            case GetExpression getExpression:
                ResolveExpression(getExpression);
                return;
            case SuperExpression superExpression:
                ResolveExpression(superExpression);
                return;
            case ThisExpression thisExpression:
                ResolveExpression(thisExpression);
                return;
            case SetExpression setExpression:
                ResolveExpression(setExpression);
                return;
            case VariableExpression variableExpression:
                ResolveExpression(variableExpression);
                return;
            case AssignmentExpression assignmentExpression:
                ResolveExpression(assignmentExpression);
                return;
            case BinaryExpression binaryExpression:
                ResolveExpression(binaryExpression);
                return;
            case CallExpression callExpression:
                ResolveExpression(callExpression);
                return;
            case GroupingExpression groupingExpression:
                ResolveExpression(groupingExpression);
                return;
            case LogicalExpression logicalExpression:
                ResolveExpression(logicalExpression);
                return;
            case UnaryExpression unaryExpression:
                ResolveExpression(unaryExpression);
                return;
            default:
                throw new NotImplementedException();
        }
    }
    
    /// <summary>
    /// Resolves a super expression
    /// </summary>
    /// <param name="superExpression"></param>
    private static void ResolveExpression(SuperExpression superExpression)
    {
        if (currentClassType is ClassType.NONE)
        {
            ErrorLogger.Error(superExpression.Token, "Can't use \'super\' outside of a class");
        }
        else if (currentClassType is not ClassType.SUBCLASS)
        {
            ErrorLogger.Error(superExpression.Token, "Can't use \'super\' with no superclass");
        }
        ResolveLocal(superExpression, superExpression.Token);
    }

    /// <summary>
    /// Resolves a ThisExpression
    /// </summary>
    /// <param name="thisExpression">The ThisExpression that shall be resolved</param>
    private static void ResolveExpression(ThisExpression thisExpression)
    {
        if (currentClassType is ClassType.NONE)
        {
            ErrorLogger.Error(thisExpression.Token, "Cant use \'this\' outside of a class");
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
            ErrorLogger.Error(variableExpression.Token, "Can't read local variable in it's own initializer");
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
                KelloxInterpreter.Resolve(expression, scopes.Count - 1 - i);
            }
            i--;
        }
    }

    #endregion Expressions

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
            ErrorLogger.Error(identifierToken.Line, "A variable with this name (" + identifierToken.Lexeme + ") was already defined in this scope");
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

    #endregion DeclaringAndDefining

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

    #endregion ScopeHandling
}
