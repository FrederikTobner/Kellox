namespace Interpreter
{
    internal class Token
    {
        public string Lexeme { get; init; }

        public object? Literal { get; init; }

        public int Line { get; init; }

        internal TokenType TokenType { get; init; }


        public Token(TokenType tokenType, string lexeme, object? literal, int line)
        {
            this.TokenType = tokenType;
            this.Lexeme = lexeme;
            this.Literal = literal;
            this.Line = line;
        }
        public override string ToString() => TokenType.ToString() + " " + Lexeme + " " + Literal;
    }
}
