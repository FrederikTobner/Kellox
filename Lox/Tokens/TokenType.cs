﻿namespace Lox.Tokens;

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
    BANG,
    EQUAL,
    GREATER,
    LESS,

    // two character tokens.
    BANG_EQUAL,
    EQUAL_EQUAL,
    GREATER_EQUAL,
    LESS_EQUAL,
    PLUS_PLUS,
    MINUS_MINUS,
    PLUS_EQUAL,
    MINUS_EQUAL,
    STAR_EQUAL,
    SLASH_EQUAL,

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
