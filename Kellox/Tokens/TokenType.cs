namespace Kellox.Tokens;

/// <summary>
/// The different Types of Tokens in the Kellox
/// </summary>
internal enum TokenType
{
    #region SingleCharacterTokens

    /// <summary>
    /// (
    /// </summary>
    LEFT_PARENTHESIS,

    /// <summary>
    /// )
    /// </summary>
    RIGHT_PARENTHESIS,

    /// <summary>
    /// {
    /// </summary>
    LEFT_BRACE,

    /// <summary>
    /// }
    /// </summary>
    RIGHT_BRACE,

    /// <summary>
    /// ,
    /// </summary>
    COMMA,

    /// <summary>
    /// .
    /// </summary>
    DOT,

    /// <summary>
    /// -
    /// </summary>
    MINUS,

    /// <summary>
    /// +
    /// </summary>
    PLUS,

    /// <summary>
    /// ;
    /// </summary>
    SEMICOLON,

    /// <summary>
    /// /
    /// </summary>
    SLASH,

    /// <summary>
    /// *
    /// </summary>
    STAR,

    /// <summary>
    /// *
    /// </summary>
    BANG,

    /// <summary>
    /// !
    /// </summary>
    EQUAL,

    /// <summary>
    /// >
    /// </summary>
    GREATER,

    /// <summary>
    /// <
    /// </summary>
    LESS,

    /// <summary>
    /// :
    /// </summary>
    DOUBLEDOT,

    #endregion SingleCharacterTokens

    #region TwoCharacterTokens

    /// <summary>
    /// !=
    /// </summary>
    BANG_EQUAL,

    /// <summary>
    /// ==
    /// </summary>
    EQUAL_EQUAL,

    /// <summary>
    /// >=
    /// </summary>
    GREATER_EQUAL,

    /// <summary>
    /// <=
    /// </summary>
    LESS_EQUAL,

    /// <summary>
    /// ++
    /// </summary>
    PLUS_PLUS,

    /// <summary>
    /// --
    /// </summary>
    MINUS_MINUS,

    /// <summary>
    /// +=
    /// </summary>
    PLUS_EQUAL,

    /// <summary>
    /// -=
    /// </summary>
    MINUS_EQUAL,

    /// <summary>
    /// *=
    /// </summary>
    STAR_EQUAL,

    /// <summary>
    /// /=
    /// </summary>
    SLASH_EQUAL,

    /// <summary>
    /// Identifier of a variable/class/function
    /// </summary>
    IDENTIFIER,

    #endregion TwoCharacterTokens

    #region Literals

    /// <summary>
    /// A string literal -> "a"
    /// </summary>
    STRING,

    /// <summary>
    /// A number literal -> 5 or 5.2
    /// </summary>
    NUMBER,

    /// <summary>
    /// A boolean literal -> true
    /// </summary>
    TRUE,

    /// <summary>
    /// A boolean literal -> false
    /// </summary>
    FALSE,

    /// <summary>
    /// A string literal -> "a"
    /// </summary>
    NIL,

    #endregion Literals

    #region Keywords

    /// <summary>
    /// and keyword, for creating an and statement -> "and"
    /// </summary>
    AND,

    /// <summary>
    /// or keyword, for creating an or statement -> "or"
    /// </summary>
    OR,

    /// <summary>
    /// if keyword, for creating an if statement -> "if"
    /// </summary>
    IF,

    /// <summary>
    /// else keyword, for creating an elsebranch to for an if statement -> "else"
    /// </summary>
    ELSE,

    /// <summary>
    /// while keyword, for creating a while loop -> "while"
    /// </summary>
    WHILE,

    /// <summary>
    /// for keyword, for creating a for loop -> "for"
    /// </summary>
    FOR,

    /// <summary>
    /// break keyword, for stoping a loop -> "break" 🛑
    /// </summary>
    BREAK,

    /// <summary>
    /// continue keyword, for forcing the next iterration of a loop -> "continue"
    /// </summary>
    CONTINUE,

    /// <summary>
    /// var keyword, for declaring a variable -> "var"
    /// </summary>
    VAR,


    #region FunctionSpecific

    /// <summary>
    /// fun keyword for declaring a function -> "fun"
    /// </summary>
    FUN,

    /// <summary>
    /// print keyword -> "print"
    /// </summary>
    PRINT,

    /// <summary>
    /// println keyword -> "println"
    /// </summary>
    PRINTLN,

    /// <summary>
    /// return keyword -> "return"
    /// </summary>
    RETURN,

    #endregion FunctionSpecific

    #region ClassSpecific

    /// <summary>
    /// super keyword -> "super"
    /// </summary>
    SUPER,

    /// <summary>
    /// class keyword -> "class"
    /// </summary>
    CLASS,

    /// <summary>
    /// this keyword -> "this"
    /// </summary>
    THIS,

    #endregion ClassSpecific

    #endregion Keywords

    #region FileSpecific
    /// <summary>
    /// Marks the end of a file
    /// </summary>
    EOF

    #endregion FileSpecific
}
