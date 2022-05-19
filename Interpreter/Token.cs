namespace Interpreter
{
    internal class Token
    {
        readonly TokenType tokenType;
        readonly string lexeme;
        readonly object? literal;
        readonly int line;

        public Token(TokenType tokenType, string lexeme, object? literal, int line)
        {
            this.tokenType = tokenType;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }

        public override string ToString() => tokenType.ToString() + " " + lexeme + " " + literal;
    }
}
