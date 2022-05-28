using Interpreter.Exceptions;
using Interpreter.Expressions;
using Interpreter.Statements;

namespace Interpreter
{
    /// <summary>
    /// Takes a flat sequence of tokens and builds a syntax tree based on the tokens
    /// </summary>
    internal partial class Parser
    {
        /// <summary>
        /// Flat sequence of Tokens
        /// </summary>
        private readonly List<Token> tokens;

        /// <summary>
        /// The current Position in the sequence of tokens
        /// </summary>
        private int current;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        /// <summary>
        /// Builds a List of Statements out of a flat sequence of Tokens
        /// </summary>
        internal List<IStatement> Parse()
        {
            List<IStatement> statements = new();
            while (!IsAtEnd())
            {
                IStatement statement = Statement();
                statements.Add(statement);
                //There is no Semicilon after a Block/IfStatement/WhileStatement
                if (statement is not BlockStatement && statement is not IfStatement && statement is not WhileStatement)
                {
                    Consume(TokenType.SEMICOLON, "Expect ';' after value");
                }
            }
            return statements;
        }

        /// <summary>
        /// Determines the next Statement that shall be executed
        /// </summary>
        /// <returns></returns>
        private IStatement Statement()
        {
            if (Match(TokenType.FOR))
            {
                return CreateForStatement();
            }
            else if (Match(TokenType.IF))
            {
                return CreateIfStatement();
            }
            else if (Match(TokenType.VAR))
            {
                return CreateDeclarationStatement();

            }
            else if (Match(TokenType.PRINT))
            {
                return new PrintStatement(Expression());
            }
            else if (Match(TokenType.WHILE))
            {
                return CreateWhileStatement();
            }
            else if (Match(TokenType.LEFT_BRACE))
            {
                return new BlockStatement(ReadBlock());
            }
            else
            {
                return new ExpressionStatement(Expression());
            }
        }

        /// <summary>
        /// Creates an OrExpressionn
        /// </summary>
        private IExpression OrExpression()
        {
            IExpression expression = AndExpression();
            while (Match(TokenType.OR))
            {
                Token operatorToken = Previous();
                IExpression right = AndExpression();
                expression = new LogicalExpression(expression, right, operatorToken);
            }
            return expression;
        }

        /// <summary>
        /// Creates an AndExpressionn
        /// </summary>
        private IExpression AndExpression()
        {
            IExpression expression = Equality();
            while (Match(TokenType.AND))
            {
                Token operatorToken = Previous();
                IExpression right = Equality();
                expression = new LogicalExpression(expression, right, operatorToken);
            }
            return expression;
        }

        /// <summary>
        /// Used to handle a new Blockstatement
        /// </summary>
        private List<IStatement> ReadBlock()
        {
            List<IStatement> statements = new();

            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                IStatement statement = Statement();
                statements.Add(statement);
                if (statement is not BlockStatement)
                {
                    Consume(TokenType.SEMICOLON, "Expect \';\' after value");
                }
            }
            Consume(TokenType.RIGHT_BRACE, "Expected \'}\' after Block");
            return statements;
        }

        /// <summary>
        /// Creates a while statement
        /// </summary>
        private IStatement CreateWhileStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expect \'(\' after while.");
            IExpression condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect \')\' after condition.");
            IStatement body = Statement();
            return new WhileStatement(condition, body);
        }

        /// <summary>
        /// Creates a for statement (based on while statement - just syntax sugar))
        /// </summary>
        private IStatement CreateForStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expect \'(\' after for.");

            IStatement? initializerExpression = null;
            if (Match(TokenType.VAR))
            {
                initializerExpression = CreateDeclarationStatement();
            }
            else if (!Match(TokenType.SEMICOLON))
            {
                initializerExpression = new ExpressionStatement(Expression());
            }

            IExpression? conditionalExpression = null;
            if (Check(TokenType.SEMICOLON))
            {
                Advance();
                conditionalExpression = Expression();
            }

            IExpression? incrementExpression = null;
            if (!Check(TokenType.RIGHT_PAREN))
            {
                Consume(TokenType.SEMICOLON, "Expect \';\' after loop condition");
                incrementExpression = Expression();
            }
            Consume(TokenType.RIGHT_PAREN, "Expect \')\' after for.");

            IStatement body = Statement();

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
        private IStatement CreateIfStatement()
        {
            Consume(TokenType.LEFT_PAREN, "Expect \'(\' after \'if\'.");
            IExpression condition = Expression();
            Consume(TokenType.RIGHT_PAREN, "Expect \')\' after if condition.");
            IStatement thenBranch = Statement();
            IStatement? elseBranch = null;
            if (Match(TokenType.ELSE))
            {
                elseBranch = Statement();
            }
            return new IfStatement(condition, thenBranch, elseBranch);
        }

        /// <summary>
        /// Used to handle a new Declarationstatement
        /// </summary>
        private IStatement CreateDeclarationStatement()
        {
            Token token = Consume(TokenType.IDENTIFIER, "Expect variable name");
            current++;
            return new DeclarationStatement(token, Expression());

        }

        /// <summary>
        /// Used to handle a new Expressionstatement
        /// </summary>
        private IExpression Expression() => Assignment();

        /// <summary>
        /// Used to handle a new AssignmentStatement
        /// </summary>
        private IExpression Assignment()
        {
            IExpression expression = OrExpression();

            if (Match(TokenType.EQUAL))
            {
                Token equals = Previous();
                IExpression value = Assignment();
                if (expression is VariableExpression variableExpression)
                {
                    Token name = variableExpression.Token;
                    return new AssignmentExpression(name, value);
                }

                Error(equals, "Invalid assignment target.");
            }
            return expression;
        }

        /// <summary>
        /// Crates a new EqualityStatement (a != x/ a == x)
        /// </summary>
        private IExpression Equality()
        {
            IExpression expression = Comparison();

            while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
            {
                Token operatorToken = Previous();
                IExpression right = Comparison();
                expression = new BinaryExpression(expression, operatorToken, right);
            }

            return expression;
        }

        /// <summary>
        /// Determines weather the next Token is from the specified TokenTypes
        /// </summary>
        /// <param name="types">Types of the Tokens</param>
        private bool Match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (Check(type))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the next Token is of a given type
        /// </summary>
        /// <param name="type">The type that shall be checked</param>
        private bool Check(TokenType type) => !IsAtEnd() && Peek().TokenType == type;

        /// <summary>
        /// Advances a position further in the flat sequence of Tokens
        /// </summary>
        private Token Advance()
        {
            if (!IsAtEnd())
            {
                current++;
            }
            return Previous();
        }

        /// <summary>
        /// Determines whether the ned of the file has been reached
        /// </summary>
        private bool IsAtEnd() => Peek().TokenType == TokenType.EOF;


        /// <summary>
        /// Returns current Token
        /// </summary>
        private Token Peek() => tokens[current];


        /// <summary>
        /// Returns the prevoius Token
        /// </summary>
        private Token Previous() => tokens[current - 1];

        /// <summary>
        ///  Creates a new Comparison Expression E.g. a > x / a >= x / a < x / a <= x
        /// </summary>
        private IExpression Comparison()
        {
            IExpression expression = Term();
            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token operatorToken = Previous();
                IExpression right = Term();
                expression = new BinaryExpression(expression, operatorToken, right);
            }
            return expression;
        }

        /// <summary>
        /// Creates a new Term expression (a + b / a - b)
        /// </summary>
        private IExpression Term()
        {
            IExpression expression = Factor();
            while (Match(TokenType.MINUS, TokenType.PLUS))
            {
                Token operatorToken = Previous();
                IExpression right = Factor();
                expression = new BinaryExpression(expression, operatorToken, right);
            }
            return expression;
        }

        /// <summary>
        /// Creates a new Factor expression (a * b / a / b)
        /// </summary>
        private IExpression Factor()
        {
            IExpression expression = Unary();
            while (Match(TokenType.SLASH, TokenType.STAR))
            {
                Token operatorToken = Previous();
                IExpression right = Unary();
                expression = new BinaryExpression(expression, operatorToken, right);
            }

            return expression;
        }

        /// <summary>
        /// Creates a new unary expression (!a / -a)
        /// </summary>
        private IExpression Unary()
        {
            if (Match(TokenType.BANG, TokenType.MINUS))
            {
                Token operatorToken = Previous();
                IExpression right = Unary();
                return new UnaryExpression(operatorToken, right);
            }

            return Primary();
        }

        /// <summary>
        /// Creates the primary types of expressions (literal, variable & group)
        /// </summary>
        private IExpression Primary()
        {
            if (Match(TokenType.FALSE))
            {
                return new LiteralExpression(false);
            }
            if (Match(TokenType.TRUE))
            {
                return new LiteralExpression(true);
            }
            if (Match(TokenType.NIL))
            {
                return new LiteralExpression(null);
            }

            if (Match(TokenType.NUMBER, TokenType.STRING))
            {
                return new LiteralExpression(Previous().Literal);
            }

            if (Match(TokenType.IDENTIFIER))
            {
                return new VariableExpression(Previous());
            }

            if (Match(TokenType.LEFT_PAREN))
            {
                IExpression expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return new GroupingExpression(expr);
            }
            throw Error(Peek(), "Expect expression");
        }

        /// <summary>
        /// Checks weather the next token is of the given type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private Token Consume(TokenType type, string message)
        {
            if (Check(type))
            {
                return Advance();
            }
            throw Error(Peek(), message);
        }


        /// <summary>
        /// Can be used to parse statements after a statment that has caused a RunTimeError
        /// </summary>
        private void Synchronize()
        {
            Advance();
            while (!IsAtEnd())
            {
                if (Previous().TokenType == TokenType.SEMICOLON) return;

                switch (Peek().TokenType)
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
                Advance();
            }
        }

        /// <summary>
        /// Logs a ParseError
        /// </summary>
        /// <param name="token">The Token that has triggered the parseError</param>
        /// <param name="message">The Message that shall be displayed</param>
        private static ParseError Error(Token token, string message)
        {
            Program.Error(token, message);
            return new ParseError();
        }
    }
}
