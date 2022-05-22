using Interpreter.Exceptions;
using Interpreter.Expr;

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

        internal Expression? Parse()
        {
            try
            {
                return Expression();
            }
            catch (Exception)
            {
                return null;
            }
        }

        private Expression Expression()
        {
            return Equality();
        }

        private Expression Equality()
        {
            Expression expression = Comparison();

            while (Match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
            {
                Token operatorToken = Previous();
                Expression right = Comparison();
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

        private Expression Comparison()
        {
            Expression expr = Term();

            while (Match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token operatorToken = Previous();
                Expression right = Term();
                expr = new BinaryExpression(expr, operatorToken, right);
            }

            return expr;
        }

        private Expression Term()
        {
            Expression expression = Factor();

            while (Match(TokenType.MINUS, TokenType.PLUS))
            {
                Token operatorToken = Previous();
                Expression right = Factor();
                expression = new BinaryExpression(expression, operatorToken, right);
            }

            return expression;
        }

        private Expression Factor()
        {
            Expression expression = Unary();

            while (Match(TokenType.SLASH, TokenType.STAR))
            {
                Token operatorToken = Previous();
                Expression right = Unary();
                expression = new BinaryExpression(expression, operatorToken, right);
            }

            return expression;
        }

        private Expression Unary()
        {
            if (Match(TokenType.BANG, TokenType.MINUS))
            {
                Token operatorToken = Previous();
                Expression right = Unary();
                return new UnaryExpression(operatorToken, right);
            }

            return Primary();
        }

        private Expression Primary()
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

            if (Match(TokenType.LEFT_PAREN))
            {
                Expression expr = Expression();
                Consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return new GroupingExpression(expr);
            }
            throw Error(Peek(), "Expect expression");
        }

        private Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();

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
                    case TokenType.FUN:
                    case TokenType.VAR:
                    case TokenType.FOR:
                    case TokenType.IF:
                    case TokenType.WHILE:
                    case TokenType.PRINT:
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
