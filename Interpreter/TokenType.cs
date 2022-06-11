namespace Interpreter
{
    /// <summary>
    /// The different Types of Tokens in the Lox
    /// </summary>
    internal enum TokenType
    {
        // Single-character tokens.
        LEFT_PARENTHESIS,
        RIGHT_PARENTHESIS,
        LEFT_BRACE,
        RIGHT_BRACE,
        COMMA,
        DOT,
        MINUS,
        PLUS,
        SEMICOLON,
        SLASH,
        STAR,

        // One or two character tokens.
        BANG,
        BANG_EQUAL,
        EQUAL,
        EQUAL_EQUAL,
        GREATER,
        GREATER_EQUAL,
        LESS,
        LESS_EQUAL,

        // Literals.
        IDENTIFIER,
        STRING,
        NUMBER,

        // Keywords.
        TRUE,
        FALSE,
        AND,
        OR,
        IF,
        ELSE,
        WHILE,
        FOR,
        VAR,
        NIL,

        //Function/method specific
        FUN,
        PRINT,
        RETURN,

        //Class specific
        SUPER,
        CLASS,
        THIS,

        // End of the file
        EOF
    }
}
