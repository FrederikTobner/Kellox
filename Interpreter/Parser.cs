using Interpreter.Exceptions;
using Interpreter.Expressions;
using Interpreter.Statements;

namespace Interpreter
{
    internal partial class Parser
    {
        private readonly List<Token> tokens;
        private int current;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        internal List<IStatement> Parse()
        {
            List<IStatement> statements = new();
            while (!IsAtEnd())
            {
                IStatement statement = Statement();
                statements.Add(statement);
                if (statement is not BlockStatement)
                {
                    Consume(TokenType.SEMICOLON, "Expect ';' after value");
                }
            }

            return statements;
        }

        private IStatement Statement()
        {
            if (Match(TokenType.VAR))
            {
                return Declaration();

            }
            else if (Match(TokenType.PRINT))
            {
                return new PrintStatement(Expression());
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

        private List<IStatement> ReadBlock()
        {
            List<IStatement> statements = new();

            while (!Check(TokenType.RIGHT_BRACE) && !IsAtEnd())
            {
                IStatement statement = Statement();
                statements.Add(statement);
                if (statement is not BlockStatement)
                {
                    Consume(TokenType.SEMICOLON, "Expect ';' after value");
                }
            }
            Consume(TokenType.RIGHT_BRACE, "Expected \'}\' after Block");
            return statements;
        }

        private IStatement Declaration()
        {
            Token token = Consume(TokenType.IDENTIFIER, "Expect variable name");
            current++;
            return new DeclarationStatement(token, Expression());

        }

        private IExpression Expression() => Assignment();

        private IExpression Assignment()
        {
            IExpression expression = Equality();

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
        /// Determines weather the next Token is from the specified TokenType
        /// </summary>
        /// <param name="types">Type of the Token</param>
        /// <returns></returns>
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
        /// Returns the type of the next Token
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool Check(TokenType type) => !IsAtEnd() && Peek().TokenType == type;

        private Token Advance()
        {
            if (!IsAtEnd())
            {
                current++;
            }
            return Previous();
        }

        private bool IsAtEnd() => Peek().TokenType == TokenType.EOF;


        /// <summary>
        /// Returns current Token
        /// </summary>
        private Token Peek() => tokens[current];


        /// <summary>
        /// Returns the prevoius Token
        /// </summary>
        private Token Previous() => tokens[current - 1];

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
        /// Checks wheather the next token is of the given type
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
        private static ParseError Error(Token token, string message)
        {
            CustomInterpreter.Error(token, message);
            return new ParseError();
        }
    }
}
