using Kellox.Exceptions;
using Kellox.Expressions;
using Kellox.Interpreter;
using Kellox.Statements;
using Kellox.Tokens;
using Kellox.Utils;

namespace Kellox.Parser;

/// <summary>
/// Takes a flat sequence of tokens and builds a syntax tree based on the tokens
/// </summary>
public static class KelloxParser
{
    /// <summary>
    /// The current Position in the sequence of tokens
    /// </summary>
    private static int current;

    /// <summary>
    /// Builds a KelloxProgram (containing an abstract syntax tree) out of a flat sequence of Tokens
    /// </summary>
    public static KelloxProgram Parse(IReadOnlyList<Token> tokens)
    {
        current = 0;
        List<IStatement> statements = new();
        while (!IsAtEnd(tokens))
        {
            try
            {
                IStatement statement = Statement(tokens);
                statements.Add(statement);
            }
            catch (ParseError error)
            {
                ErrorLogger.Error(error.ErrorToken, error.Message);
                // Synchronizes the parser after a parsing error has occured
                Synchronizer.Synchronize(tokens, ref current);
            }
        }
        return new(statements);
    }

    #region Statements

    /// <summary>
    /// Determines the next Statement that shall be executed based on the next Token
    /// </summary>
    private static IStatement Statement(IReadOnlyList<Token> tokens)
    {
        IStatement statement;
        if (Match(tokens, TokenType.FOR))
        {
            statement = CreateForStatement(tokens);
        }
        else if (Check(tokens, TokenType.BREAK))
        {
            statement = new BreakStatement(Advance(tokens));
        }
        else if (Check(tokens, TokenType.CONTINUE))
        {
            statement = new ContinueStatement(Advance(tokens));
        }
        else if (Match(tokens, TokenType.IF))
        {
            statement = CreateIfStatement(tokens);
        }
        else if (Match(tokens, TokenType.CLASS))
        {
            statement = CreateClassStatement(tokens);
        }
        else if (Match(tokens, TokenType.FUN))
        {
            statement = CreateFunctionStatement(tokens, "function");
        }
        else if (Match(tokens, TokenType.VAR))
        {
            statement = CreateDeclarationStatement(tokens);
        }
        else if (Check(tokens, TokenType.PRINT, TokenType.PRINTLN))
        {
            Token printToken = Advance(tokens);
            statement = new PrintStatement(Expression(tokens), printToken.TokenType is TokenType.PRINTLN);
        }
        else if (Check(tokens, TokenType.RETURN))
        {
            statement = CreateReturnStatement(tokens);
        }
        else if (Match(tokens, TokenType.WHILE))
        {
            statement = CreateWhileStatement(tokens);
        }
        else if (Match(tokens, TokenType.LEFT_BRACE))
        {
            statement = new BlockStatement(ReadBlock(tokens));
        }
        else
        {
            statement = new ExpressionStatement(Expression(tokens));
        }

        if (StatementConsumesSemicolon(statement))
        {
            Consume(tokens, TokenType.SEMICOLON, "Expect \';\' after value");
        }
        return statement;
    }

    /// <summary>
    /// Determines whether a statement is Consuming a semicolon
    /// </summary>
    /// <param name="statement">The statement that shall be evaluated</param>
    private static bool StatementConsumesSemicolon(IStatement statement) => statement is DeclarationStatement or ExpressionStatement or PrintStatement or BreakStatement or ContinueStatement;

    /// <summary>
    /// Creates a new Return statement
    /// </summary>
    /// <returns></returns>
    private static IStatement CreateReturnStatement(IReadOnlyList<Token> tokens)
    {
        Token keyword = Advance(tokens);
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
        if (Match(tokens, TokenType.DOUBLEDOT))
        {
            superClass = new VariableExpression(Consume(tokens, TokenType.IDENTIFIER, "Expet superclass name after \':\'"));
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
    /// Creates a new Function statement (declaration not calle)
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
            Consume(tokens, TokenType.SEMICOLON, "Expect \';\' after loop initializer");
        }
        else if (!Match(tokens, TokenType.SEMICOLON))
        {
            initializerExpression = new ExpressionStatement(Expression(tokens));
        }

        IExpression? conditionalExpression = null;
        if (!Match(tokens, TokenType.SEMICOLON))
        {
            conditionalExpression = Expression(tokens);
            Consume(tokens, TokenType.SEMICOLON, "Expect \';\' after loop condition");
        }        

        IExpression? incrementExpression = null;
        if (!Check(tokens, TokenType.RIGHT_PARENTHESIS))
        {
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
            //If there is no Condition specified it is always true
            conditionalExpression = new LiteralExpression(true);
        }
        body = new WhileStatement(conditionalExpression, body, incrementExpression is null ? null : new ExpressionStatement(incrementExpression));
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
        Advance(tokens);
        return new DeclarationStatement(token, Expression(tokens));
    }

    #endregion Statements

    #region Expressions

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
        //Creates an Assignment expression
        if (Check(tokens, TokenType.EQUAL, TokenType.PLUS_PLUS, TokenType.MINUS_MINUS, TokenType.PLUS_EQUAL, TokenType.MINUS_EQUAL, TokenType.STAR_EQUAL, TokenType.SLASH_EQUAL))
        {
            IExpression value;
            Token operatorToken = Advance(tokens);
            if (operatorToken.TokenType is TokenType.EQUAL)
            {
                //Normal Assignment
                value = Assignment(tokens);
            }
            else if (operatorToken.TokenType is TokenType.PLUS_PLUS)
            {
                //Increment                
                value = new BinaryExpression(expression, new Token(TokenType.PLUS, "+", null, operatorToken.Line), new LiteralExpression(1.0));
            }
            else if (operatorToken.TokenType is TokenType.MINUS_MINUS)
            {
                //Decrement
                value = new BinaryExpression(expression, new Token(TokenType.MINUS, "-", null, operatorToken.Line), new LiteralExpression(1.0));
            }
            else
            {
                // +=, -=, *=, /=
                value = CreateXEqualExpression(tokens, operatorToken, expression);
            }
            if (expression is VariableExpression variableExpression)
            {
                Token name = variableExpression.Token;
                return new AssignmentExpression(name, value);
            }
            // Assignnment target is a get expression e.g. rect.x = 10;
            else if (expression is GetExpression getExpression)
            {
                return new SetExpression(getExpression.Object, getExpression.Name, value);
            }
            throw new ParseError(operatorToken, "Invalid assignment target: " + expression.ToString());
        }
        return expression;
    }

    /// <summary>
    /// Creates a new XEqual expression e.g. x+=3/x-=3/x*=3/x/=3
    /// </summary>
    /// <param name="tokens">The linear sequence of tokens that is used to create a abstract syntax tree</param>
    /// <param name="token">The Token of the Expression (PLUS_EQUAL/MINUS_EQUAL/STAR_EQUAL/SLASH_EQUAL)</param>
    /// <param name="expression">The expression left of the Operator</param>
    /// <exception cref="ParseError">Thrown if the operator is not supported</exception>
    private static IExpression CreateXEqualExpression(IReadOnlyList<Token> tokens, Token token, IExpression expression) => token.TokenType switch
    {
        TokenType.PLUS_EQUAL => new BinaryExpression(expression, new Token(TokenType.PLUS, "+", null, token.Line), Expression(tokens)),
        TokenType.MINUS_EQUAL => new BinaryExpression(expression, new Token(TokenType.MINUS, "-", null, token.Line), Expression(tokens)),
        TokenType.STAR_EQUAL => new BinaryExpression(expression, new Token(TokenType.STAR, "*", null, token.Line), Expression(tokens)),
        TokenType.SLASH_EQUAL => new BinaryExpression(expression, new Token(TokenType.SLASH, "/", null, token.Line), Expression(tokens)),
        _ => throw new ParseError(token, "Invalid Operator"),
    };

    /// <summary>
    /// Creates an OrExpressionn
    /// </summary>
    private static IExpression OrExpression(IReadOnlyList<Token> tokens)
    {
        IExpression expression = AndExpression(tokens);
        while (Check(tokens, TokenType.OR))
        {
            Token operatorToken = Advance(tokens);
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
        while (Check(tokens, TokenType.AND))
        {
            Token operatorToken = Advance(tokens);
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

        while (Check(tokens, TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
        {
            Token operatorToken = Advance(tokens);
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
        while (Check(tokens, TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
        {
            Token operatorToken = Advance(tokens);
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
        while (Check(tokens, TokenType.MINUS, TokenType.PLUS))
        {
            Token operatorToken = Advance(tokens);
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
        while (Check(tokens, TokenType.SLASH, TokenType.STAR))
        {
            Token operatorToken = Advance(tokens);
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
        if (Check(tokens, TokenType.BANG, TokenType.MINUS))
        {
            Token operatorToken = Advance(tokens);
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
                    throw new ParseError(Peek(tokens), "Can't have more than 255 parameters.");
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
        if (Check(tokens, TokenType.NUMBER))
        {
            return new LiteralExpression(Advance(tokens).Literal);
        }
        if (Check(tokens, TokenType.STRING))
        {
            Token stringLiteralToken = Advance(tokens);
            return new LiteralExpression(stringLiteralToken.Literal, stringLiteralToken);
        }
        if (Check(tokens, TokenType.SUPER))
        {
            Token keyword = Advance(tokens);
            Consume(tokens, TokenType.DOT, "Expect \'.\' after \'super\'");
            Token method = Consume(tokens, TokenType.IDENTIFIER, "Expect superclass method name");
            return new SuperExpression(keyword, method);
        }
        if (Check(tokens, TokenType.THIS))
        {
            return new ThisExpression(Advance(tokens));
        }
        if (Check(tokens, TokenType.IDENTIFIER))
        {
            return new VariableExpression(Advance(tokens));
        }

        if (Match(tokens, TokenType.LEFT_PARENTHESIS))
        {
            IExpression expr = Expression(tokens);
            Consume(tokens, TokenType.RIGHT_PARENTHESIS, "Expect \')\' after expression.");
            return new GroupingExpression(expr);
        }
        throw new ParseError(Peek(tokens), "Expect expression");
    }

    #endregion Expressions

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
    private static bool Check(IReadOnlyList<Token> tokens, params TokenType[] tokenTypes)
    {
        foreach (TokenType type in tokenTypes)
        {
            if (!IsAtEnd(tokens) && Peek(tokens).TokenType == type)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Advances a position further in the flat sequence of Tokens and returns the prevoius token
    /// </summary>
    private static Token Advance(IReadOnlyList<Token> tokens)
    {
        if (!IsAtEnd(tokens))
        {
            Token token = Peek(tokens);
            current++;
            return token;
        }
        return Peek(tokens);

    }

    /// <summary>
    /// Determines whether the ned of the file has been reached
    /// </summary>
    private static bool IsAtEnd(IReadOnlyList<Token> tokens) => Peek(tokens).TokenType is TokenType.EOF;

    /// <summary>
    /// Returns current Token
    /// </summary>
    private static Token Peek(IReadOnlyList<Token> tokens) => tokens[current];

    /// <summary>
    /// Checks weather the next token is of the given type
    /// </summary>
    /// <param name="tokenType">The tokentype that shall be checked</param>
    /// <param name="message">The Message of the exception, that is thrown if the next token is not of the given type</param>
    private static Token Consume(IReadOnlyList<Token> tokens, TokenType tokenType, string message)
    {
        if (!Check(tokens, tokenType))
        {
            throw new ParseError(Peek(tokens), message);
        }
        return Advance(tokens);

    }
}
