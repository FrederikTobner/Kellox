using Lox.Functions;
using Lox.i18n;
using Lox.Interpreter.Exceptions;
using Lox.Tokens;

namespace Lox.Expressions;

/// <summary>
/// Models a CallExpression in Lox
/// </summary>
internal class CallExpression : IExpression
{
    

    /// <summary>
    /// The Expression that was called e.g. rect(5, 6).area() -> 'rect(5, 6)'
    /// </summary>
    public IExpression Calle { get; init; }

    /// <summary>
    /// Contains the Right_Parenthesis -> ')'
    /// Only used for Error Logging
    /// </summary>
    public Token Paren { get; init; }

    /// <summary>
    /// Arguments of the Call e.g. wait(1) -> 1
    /// </summary>
    public IReadOnlyList<IExpression> Arguments { get; init; }

    public CallExpression(IExpression calle, Token paren, List<IExpression> arguments)
    {
        this.Calle = calle;
        this.Paren = paren;
        this.Arguments = arguments;
    }

    public object? EvaluateExpression()
    {
        object? callee = Calle.EvaluateExpression();

        List<object?> arguments = new();

        foreach (IExpression expression in Arguments)
        {
            arguments.Add(expression.EvaluateExpression());
        }
        if (callee is IFunction function)
        {
            if (arguments.Count != function.Arity)
            {
                throw new RunTimeError(this.Paren, "Expected " + function.Arity + " argumnets but got " + arguments.Count + ".");
            }
            return function?.Call(arguments);
        }
        throw new RunTimeError(this.Paren, Messages.CallOnNonFunctionOrClassErrorMessage);
    }

    public override string ToString() => IExpression.Parenthesize(this.Calle.ToString(), this.Arguments.ToArray());
}
