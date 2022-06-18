using Kellox.Classes;
using Kellox.Exceptions;
using Kellox.Functions;
using Kellox.i18n;
using Kellox.Interpreter;
using Kellox.Tokens;

namespace Kellox.Expressions;

internal class SuperExpression : IExpression
{
    /// <summary>
    /// The Token of the super Expression -> always super
    /// </summary>
    public Token Token { get; init; }

    /// <summary>
    /// The Method that was called by the super Expression -> e.g. init
    /// </summary>
    private readonly Token method;

    public SuperExpression(Token Token, Token method)
    {
        this.Token = Token;
        this.method = method;
    }

    public object? EvaluateExpression()
    {
        KelloxInterpreter.TryGetDepthOfLocal(this, out int distance);
        KelloxClass? superClass = (KelloxClass?)KelloxInterpreter.currentEnvironment.GetAt(distance, Token);
        if (superClass is null)
        {
            throw new RunTimeError(this.method, Messages.ThereIsNoSuperClassDefinedErrorMessage);
        }
        KelloxInstance? instance = (KelloxInstance?)KelloxInterpreter.currentEnvironment.GetAt(distance - 1, new Token(TokenType.THIS, "this", null, 0));
        if (instance is null)
        {
            throw new RunTimeError(this.method, "instance is nil");
        }
        if (!superClass.TryFindMethod(this.method.Lexeme, out KelloxFunction? function))
        {
            throw new RunTimeError(this.method, "Undefiended method \'" + method.Lexeme + "\'in superclass.");
        }
        return function?.Bind(instance);
    }

    public override string ToString() => Token.Lexeme + '.' + method.Lexeme;
}
