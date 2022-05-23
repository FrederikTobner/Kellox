using Interpreter.Exceptions;
using Interpreter.Expr;
using Interpreter.Stmt;

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
                if (Match(TokenType.VAR))
                {
                    Token token = Consume(TokenType.IDENTIFIER, "Expect variable name");
                    current++;
                    statements.Add(new DeclarationStatement(token, Expression()));
                    Consume(TokenType.SEMICOLON, "Expect ';' after value");
                }
                else if (Match(TokenType.PRINT))
                {
                    statements.Add(new PrintStatement(Expression()));
                    Consume(TokenType.SEMICOLON, "Expect ';' after value");
                }
                else
                {
                    statements.Add(new ExpressionStatement(Expression()));
                    Consume(TokenType.SEMICOLON, "Expect ';' after value");
                }
            }

            return statements;
        }

        private IExpression Expression()
        {
            return Assignment();
        }
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

        private bool Check(TokenType type)
        {
            if (IsAtEnd())
            {
                return false;
            }
            return Peek().TokenType == type;
        }

        private Token Advance()
        {
            if (!IsAtEnd())
            {
                current++;
            }
            return Previous();
        }

        private bool IsAtEnd() => Peek().TokenType == TokenType.EOF;

        private Token Peek() => tokens[current];

        private Token Previous() => tokens[current - 1];

        private IExpression Comparison()
        {
            IExpression expr = Term();

            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token operatorToken = Previous();
                IExpression right = Term();
                expr = new BinaryExpression(expr, operatorToken, right);
            }

            return expr;
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
            Program.Error(token, message);
            return new ParseError();
        }
    }
}
