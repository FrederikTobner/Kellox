﻿using Lox.Expressions;
using Lox.Interpreter;
using Lox.Parser.Exceptions;
using Lox.Statements;
using Lox.Tokens;
using Lox.Utils;

namespace Lox.Parser;

/// <summary>
/// Takes a flat sequence of tokens and builds a syntax tree based on the tokens
/// </summary>
internal static class LoxParser
{
    /// <summary>
    /// The current Position in the sequence of tokens
    /// </summary>
    private static int current;

    /// <summary>
    /// Builds a LoxProgramm out of a flat sequence of Tokens
    /// </summary>
    internal static LoxProgram Parse(IReadOnlyList<Token> tokens)
    {
        current = 0;
        List<IStatement> statements = new();
        while (!IsAtEnd(tokens))
        {
            try
            {
                IStatement statement = Statement(tokens);
                statements.Add(statement);
                if (StatementConsumesSemicolon(statement))
                {
                    Consume(tokens, TokenType.SEMICOLON, "Expect ';' after value");
                }
            }
            catch (ParseError error)
            {
                LoxErrorLogger.Error(error.ErrorToken, error.Message);
                Synchronize(tokens);
            }
        }
        return new(statements);
    }

    /// <summary>
    /// Determines the next Statement that shall be executed based on the next Token
    /// </summary>
    private static IStatement Statement(IReadOnlyList<Token> tokens)
    {
        if (Match(tokens, TokenType.FOR))
        {
            return CreateForStatement(tokens);
        }
        else if (Match(tokens, TokenType.IF))
        {
            return CreateIfStatement(tokens);
        }
        else if (Match(tokens, TokenType.CLASS))
        {
            return CreateClassStatement(tokens);
        }
        else if (Match(tokens, TokenType.FUN))
        {
            return CreateFunctionStatement(tokens, "function");
        }
        else if (Match(tokens, TokenType.VAR))
        {
            return CreateDeclarationStatement(tokens);
        }
        else if (Match(tokens, TokenType.PRINT))
        {
            return new PrintStatement(Expression(tokens));
        }
        else if (Match(tokens, TokenType.RETURN))
        {
            return CreateReturnStatement(tokens);
        }
        else if (Match(tokens, TokenType.WHILE))
        {
            return CreateWhileStatement(tokens);
        }
        else if (Match(tokens, TokenType.LEFT_BRACE))
        {
            return new BlockStatement(ReadBlock(tokens));
        }
        else
        {
            return new ExpressionStatement(Expression(tokens));
        }
    }

    /// <summary>
    /// Determines whether a statement is Consuming a semicolon
    /// </summary>
    /// <param name="statement">The statement that shall be evaluated</param>
    private static bool StatementConsumesSemicolon(IStatement statement) => statement is DeclarationStatement or ExpressionStatement or PrintStatement;

    /// <summary>
    /// Creates a new Return statement
    /// </summary>
    /// <returns></returns>
    private static IStatement CreateReturnStatement(IReadOnlyList<Token> tokens)
    {
        Token keyword = Previous(tokens);
        IExpression? value = null;
        if (!Check(tokens, TokenType.SEMICOLON))
        {
            value = Expression(tokens);
        }
        Consume(tokens, TokenType.SEMICOLON, "Expect \';\' after return value");
        return new ReturnStatement(keyword, value);
    }

    /// <summary>
    /// Used to handle a new BlockStatement
    /// </summary>
    private static List<IStatement> ReadBlock(IReadOnlyList<Token> tokens)
    {
        List<IStatement> statements = new();

        while (!Check(tokens, TokenType.RIGHT_BRACE) && !IsAtEnd(tokens))
        {
            IStatement statement = Statement(tokens);
            statements.Add(statement);
            if (StatementConsumesSemicolon(statement))
            {
                Consume(tokens, TokenType.SEMICOLON, "Expect \';\' after value");
            }
        }
        Consume(tokens, TokenType.RIGHT_BRACE, "Expected \'}\' after Block");
        return statements;
    }

    /// <summary>
    /// Creates a new Class statement
    /// </summary>
    private static IStatement CreateClassStatement(IReadOnlyList<Token> tokens)
    {
        Token name = Consume(tokens, TokenType.IDENTIFIER, "Expect class name.");
        VariableExpression? superClass = null;
        if (Match(tokens, TokenType.LESS))
        {
            Consume(tokens, TokenType.IDENTIFIER, "Expet superclass name.");
            superClass = new VariableExpression(Previous(tokens));
        }
        Consume(tokens, TokenType.LEFT_BRACE, "Expect \'{\' before class body.");
        List<FunctionStatement> methods = new();
        while (!Check(tokens, TokenType.RIGHT_BRACE))
        {
            methods.Add((FunctionStatement)CreateFunctionStatement(tokens, "method"));
        }
        Consume(tokens, TokenType.RIGHT_BRACE, "Expect \'}\' after class body.");
        return new ClassStatement(name, methods, superClass);
    }

    /// <summary>
    /// Creates a new Function statement (declaration  not calle)
    /// </summary>
    private static IStatement CreateFunctionStatement(IReadOnlyList<Token> tokens, string kind)
    {
        Token name = Consume(tokens, TokenType.IDENTIFIER, "Expect " + kind + " name.");
        Consume(tokens, TokenType.LEFT_PARENTHESIS, "Expect \'(\' after " + kind + " name.");
        //Handles arguments/parameters
        List<Token> parameters = new();
        if (!Check(tokens, TokenType.RIGHT_PARENTHESIS))
        {
            do
            {
                if (parameters.Count >= 255)
                {
                    throw new ParseError(Peek(tokens), "Can't have more than 255 parameters.");
                }
                parameters.Add(Consume(tokens, TokenType.IDENTIFIER, "Expect parameter name."));

            } while (Match(tokens, TokenType.COMMA));
        }
        Consume(tokens, TokenType.RIGHT_PARENTHESIS, "Expect \')\' after parameters");
        //Handles body
        Consume(tokens, TokenType.LEFT_BRACE, "Expect \'}\' before " + kind + " body");
        List<IStatement> body = ReadBlock(tokens);
        return new FunctionStatement(name, parameters, body);
    }

    /// <summary>
    /// Creates a while statement
    /// </summary>
    private static IStatement CreateWhileStatement(IReadOnlyList<Token> tokens)
    {
        Consume(tokens, TokenType.LEFT_PARENTHESIS, "Expect \'(\' after while.");
        IExpression condition = Expression(tokens);
        Consume(tokens, TokenType.RIGHT_PARENTHESIS, "Expect \')\' after condition.");
        IStatement body = Statement(tokens);
        return new WhileStatement(condition, body);
    }

    /// <summary>
    /// Creates a for statement (based on while statement - just syntax sugar)
    /// </summary>
    private static IStatement CreateForStatement(IReadOnlyList<Token> tokens)
    {
        Consume(tokens, TokenType.LEFT_PARENTHESIS, "Expect \'(\' after for.");

        IStatement? initializerExpression = null;
        if (Match(tokens, TokenType.VAR))
        {
            initializerExpression = CreateDeclarationStatement(tokens);
        }
        else if (!Match(tokens, TokenType.SEMICOLON))
        {
            initializerExpression = new ExpressionStatement(Expression(tokens));
        }

        IExpression? conditionalExpression = null;
        if (Check(tokens, TokenType.SEMICOLON))
        {
            Advance(tokens);
            conditionalExpression = Expression(tokens);
        }

        IExpression? incrementExpression = null;
        if (!Check(tokens, TokenType.RIGHT_PARENTHESIS))
        {
            Consume(tokens, TokenType.SEMICOLON, "Expect \';\' after loop condition");
            incrementExpression = Expression(tokens);
        }
        Consume(tokens, TokenType.RIGHT_PARENTHESIS, "Expect \')\' after for.");

        IStatement body = Statement(tokens);

        if (incrementExpression is not null)
        {
            //Adds the increment-expression at the end of the while body if it is not null
            IStatement[] statements = { body, new ExpressionStatement(incrementExpression) };
            body = new BlockStatement(statements.ToList());
        }
        if (conditionalExpression is null)
        {
            //If there is no Ccondition specified it is always true
            conditionalExpression = new LiteralExpression(true);
        }
        body = new WhileStatement(conditionalExpression, body);
        if (initializerExpression is not null)
        {
            IStatement[] statements = { initializerExpression, body };
            body = new BlockStatement(statements.ToList());
        }
        return body;
    }

    /// <summary>
    /// Used to handle a new IfStatement
    /// </summary>
    private static IStatement CreateIfStatement(IReadOnlyList<Token> tokens)
    {
        Consume(tokens, TokenType.LEFT_PARENTHESIS, "Expect \'(\' after \'if\'.");
        IExpression condition = Expression(tokens);
        Consume(tokens, TokenType.RIGHT_PARENTHESIS, "Expect \')\' after if condition.");
        IStatement thenBranch = Statement(tokens);
        IStatement? elseBranch = null;
        if (Match(tokens, TokenType.ELSE))
        {
            elseBranch = Statement(tokens);
        }
        return new IfStatement(condition, thenBranch, elseBranch);
    }

    /// <summary>
    /// Used to handle a new Declarationstatement
    /// </summary>
    private static IStatement CreateDeclarationStatement(IReadOnlyList<Token> tokens)
    {
        Token token = Consume(tokens, TokenType.IDENTIFIER, "Expect variable name");
        if (Check(tokens, TokenType.SEMICOLON))
        {
            // No value assigned -> value is null
            return new DeclarationStatement(token);
        }
        current++;
        return new DeclarationStatement(token, Expression(tokens));
    }


    /// <summary>
    /// Used to handle a new Expressionstatement
    /// </summary>
    private static IExpression Expression(IReadOnlyList<Token> tokens) => Assignment(tokens);

    /// <summary>
    /// Used to handle a new AssignmentStatement
    /// </summary>
    private static IExpression Assignment(IReadOnlyList<Token> tokens)
    {
        IExpression expression = OrExpression(tokens);

        if (Match(tokens, TokenType.EQUAL))
        {
            Token equals = Previous(tokens);
            IExpression value = Assignment(tokens);
            if (expression is VariableExpression variableExpression)
            {
                Token name = variableExpression.Token;
                return new AssignmentExpression(name, value);
            }
            else if (expression is GetExpression getExpression)
            {
                return new SetExpression(getExpression.Object, getExpression.Name, value);
            }
            throw new ParseError(equals, "Invalid assignment target.");
        }
        return expression;
    }

    /// <summary>
    /// Creates an OrExpressionn
    /// </summary>
    private static IExpression OrExpression(IReadOnlyList<Token> tokens)
    {
        IExpression expression = AndExpression(tokens);
        while (Match(tokens, TokenType.OR))
        {
            Token operatorToken = Previous(tokens);
            IExpression right = AndExpression(tokens);
            expression = new LogicalExpression(expression, right, operatorToken);
        }
        return expression;
    }

    /// <summary>
    /// Creates an AndExpressionn
    /// </summary>
    private static IExpression AndExpression(IReadOnlyList<Token> tokens)
    {
        IExpression expression = Equality(tokens);
        while (Match(tokens, TokenType.AND))
        {
            Token operatorToken = Previous(tokens);
            IExpression right = Equality(tokens);
            expression = new LogicalExpression(expression, right, operatorToken);
        }
        return expression;
    }


    /// <summary>
    /// Crates a new EqualityStatement (a != x/ a == x)
    /// </summary>
    private static IExpression Equality(IReadOnlyList<Token> tokens)
    {
        IExpression expression = Comparison(tokens);

        while (Match(tokens, TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
        {
            Token operatorToken = Previous(tokens);
            IExpression right = Comparison(tokens);
            expression = new BinaryExpression(expression, operatorToken, right);
        }

        return expression;
    }

    /// <summary>
    ///  Creates a new Comparison Expression E.g. a > x / a >= x / a < x / a <= x
    /// </summary>
    private static IExpression Comparison(IReadOnlyList<Token> tokens)
    {
        IExpression expression = Term(tokens);
        while (Match(tokens, TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
        {
            Token operatorToken = Previous(tokens);
            IExpression right = Term(tokens);
            expression = new BinaryExpression(expression, operatorToken, right);
        }
        return expression;
    }

    /// <summary>
    /// Creates a new Term expression (a + b / a - b)
    /// </summary>
    private static IExpression Term(IReadOnlyList<Token> tokens)
    {
        IExpression expression = Factor(tokens);
        while (Match(tokens, TokenType.MINUS, TokenType.PLUS))
        {
            Token operatorToken = Previous(tokens);
            IExpression right = Factor(tokens);
            expression = new BinaryExpression(expression, operatorToken, right);
        }
        return expression;
    }

    /// <summary>
    /// Creates a new Factor expression (a * b / a / b)
    /// </summary>
    private static IExpression Factor(IReadOnlyList<Token> tokens)
    {
        IExpression expression = Unary(tokens);
        while (Match(tokens, TokenType.SLASH, TokenType.STAR))
        {
            Token operatorToken = Previous(tokens);
            IExpression right = Unary(tokens);
            expression = new BinaryExpression(expression, operatorToken, right);
        }

        return expression;
    }

    /// <summary>
    /// Creates a new unary expression (!a / -a)
    /// </summary>
    private static IExpression Unary(IReadOnlyList<Token> tokens)
    {
        if (Match(tokens, TokenType.BANG, TokenType.MINUS))
        {
            Token operatorToken = Previous(tokens);
            IExpression right = Unary(tokens);
            return new UnaryExpression(operatorToken, right);
        }
        return Call(tokens);
    }

    /// <summary>
    /// Creates a new CallExpression
    /// </summary>
    private static IExpression Call(IReadOnlyList<Token> tokens)
    {
        IExpression expression = Primary(tokens);

        while (true)
        {
            if (Match(tokens, TokenType.LEFT_PARENTHESIS))
            {
                expression = FinishCall(tokens, expression);
            }
            else if (Match(tokens, TokenType.DOT))
            {
                Token name = Consume(tokens, TokenType.IDENTIFIER, "Expect property name after \'.\'.");
                expression = new GetExpression(expression, name);
            }
            else
            {
                break;
            }
        }

        return expression;
    }

    /// <summary>
    /// Adds the parameters to the call expression
    /// </summary>
    /// <param name="calle"></param>
    /// <exception cref="ParseError"></exception>
    private static IExpression FinishCall(IReadOnlyList<Token> tokens, IExpression calle)
    {
        List<IExpression> arguments = new();

        if (!Check(tokens, TokenType.RIGHT_PARENTHESIS))
        {
            do
            {
                if (arguments.Count >= 255)
                {
                    throw new ParseError(Peek(tokens), "Can't have more than 255 argumenst");
                }
                arguments.Add(Expression(tokens));
            } while (Match(tokens, TokenType.COMMA));
        }

        Token paren = Consume(tokens, TokenType.RIGHT_PARENTHESIS, "Expect \')\' after arguments.");
        return new CallExpression(calle, paren, arguments);
    }

    /// <summary>
    /// Creates the primary types of expressions (literal, variable, group, super & this)
    /// </summary>
    private static IExpression Primary(IReadOnlyList<Token> tokens)
    {
        if (Match(tokens, TokenType.FALSE))
        {
            return new LiteralExpression(false);
        }
        if (Match(tokens, TokenType.TRUE))
        {
            return new LiteralExpression(true);
        }
        if (Match(tokens, TokenType.NIL))
        {
            return new LiteralExpression(null);
        }
        if (Match(tokens, TokenType.NUMBER, TokenType.STRING))
        {
            return new LiteralExpression(Previous(tokens).Literal);
        }
        if (Match(tokens, TokenType.SUPER))
        {
            Token keyword = Previous(tokens);
            Consume(tokens, TokenType.DOT, "Expect \'.\' after \'super\'");
            Token method = Consume(tokens, TokenType.IDENTIFIER, "Expect superclass method name");
            return new SuperExpression(keyword, method);
        }
        if (Match(tokens, TokenType.THIS))
        {
            return new ThisExpression(Previous(tokens));
        }
        if (Match(tokens, TokenType.IDENTIFIER))
        {
            return new VariableExpression(Previous(tokens));
        }

        if (Match(tokens, TokenType.LEFT_PARENTHESIS))
        {
            IExpression expr = Expression(tokens);
            Consume(tokens, TokenType.RIGHT_PARENTHESIS, "Expect \')\' after expression.");
            return new GroupingExpression(expr);
        }
        throw new ParseError(Peek(tokens), "Expect expression");
    }

    /// <summary>
    /// Can be used to parse statements after a statment that has caused a RunTimeError
    /// </summary>
    private static void Synchronize(IReadOnlyList<Token> tokens)
    {
        Advance(tokens);
        while (!IsAtEnd(tokens))
        {
            if (Previous(tokens).TokenType is TokenType.SEMICOLON)
            {
                return;
            }

            switch (Peek(tokens).TokenType)
            {
                case TokenType.CLASS:
                    break;
                case TokenType.FUN:
                    break;
                case TokenType.VAR:
                    break;
                case TokenType.FOR:
                    break;
                case TokenType.IF:
                    break;
                case TokenType.WHILE:
                    break;
                case TokenType.PRINT:
                    break;
                case TokenType.RETURN:
                    return;
            }
            Advance(tokens);
        }
    }

    /// <summary>
    /// Determines weather the next Token is from the specified TokenTypes and advances a position further, if that is the case
    /// </summary>
    /// <param name="tokenTypes">Types of the Tokens</param>
    private static bool Match(IReadOnlyList<Token> tokens, params TokenType[] tokenTypes)
    {
        foreach (TokenType type in tokenTypes)
        {
            if (Check(tokens, type))
            {
                Advance(tokens);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if the next Token is of a given type
    /// </summary>
    /// <param name="tokenType">The tokentype that shall be checked</param>
    private static bool Check(IReadOnlyList<Token> tokens, TokenType tokenType) => !IsAtEnd(tokens) && Peek(tokens).TokenType == tokenType;

    /// <summary>
    /// Advances a position further in the flat sequence of Tokens
    /// </summary>
    private static Token Advance(IReadOnlyList<Token> tokens)
    {
        if (!IsAtEnd(tokens))
        {
            current++;
        }
        return Previous(tokens);
    }

    /// <summary>
    /// Determines whether the ned of the file has been reached
    /// </summary>
    private static bool IsAtEnd(IReadOnlyList<Token> tokens) => Peek(tokens).TokenType == TokenType.EOF;

    /// <summary>
    /// Returns current Token
    /// </summary>
    private static Token Peek(IReadOnlyList<Token> tokens) => tokens[current];

    /// <summary>
    /// Returns the prevoius Token
    /// </summary>
    private static Token Previous(IReadOnlyList<Token> tokens) => tokens[current - 1];

    /// <summary>
    /// Checks weather the next token is of the given type
    /// </summary>
    /// <param name="tokenType">The tokentype that shall be checked</param>
    /// <param name="message">The Message of the exception, that is thrown if the next token is not of the given type</param>
    private static Token Consume(IReadOnlyList<Token> tokens, TokenType tokenType, string message)
    {
        if (Check(tokens, tokenType))
        {
            return Advance(tokens);
        }
        throw new ParseError(Peek(tokens), message);
    }
}
