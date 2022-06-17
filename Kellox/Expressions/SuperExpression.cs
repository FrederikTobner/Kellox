using Lox.Classes;
using Lox.Functions;
using Lox.i18n;
using Lox.Interpreter;
using Lox.Interpreter.Exceptions;
using Lox.Tokens;

namespace Lox.Expressions;

internal class SuperExpression : IExpression
{
    /// <summary>
    /// The Token of the super Expression -> always super
    /// </summary>
    public Token Token { get; init; }

    /// <summary>
    /// The Method that was called by the super Expression -> e.g. 
    /// </summary>
    public Token Method { get; init; }

    public SuperExpression(Token Token, Token Method)
    {
        this.Token = Token;
        this.Method = Method;
    }

    public object? EvaluateExpression()
    {
        LoxInterpreter.TryGetDepthOfLocal(this, out int distance);
        KelloxClass? superClass = (KelloxClass?)LoxInterpreter.currentEnvironment.GetAt(distance, Token);
        if (superClass is null)
        {
            throw new RunTimeError(this.Method, Messages.ThereIsNoSuperClassDefinedErrorMessage);
        }
        KelloxInstance? instance = (KelloxInstance?)LoxInterpreter.currentEnvironment.GetAt(distance - 1, new Token(TokenType.THIS, "this", null, 0));
        KelloxFunction? function = superClass.FindMethod(this.Method.Lexeme);
        if (instance is null)
        {
            return null;
        }
        if (function is null)
        {
            throw new RunTimeError(this.Method, "Undefiended property \'" + Method.Lexeme + "\'.");
        }
        return function.Bind(instance);
    }

    public override string ToString()
    {
        return Token.Lexeme + '.' + Method.Lexeme;
    }
}
