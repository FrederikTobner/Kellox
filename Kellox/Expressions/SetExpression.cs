using Kellox.Classes;
using Kellox.i18n;
using Kellox.Interpreter.Exceptions;
using Kellox.Tokens;

namespace Kellox.Expressions;

/// <summary>
/// Models a set expression in lox e.g. rect.X = 5.0;
/// </summary>
internal class SetExpression : IExpression
{
    /// <summary>
    /// ThisExpression pointing to this object
    /// </summary>
    public IExpression Object { get; init; }

    /// <summary>
    /// The Token of the set Expression e.g for this.x = X -> 'x'
    /// </summary>
    public Token Name { get; init; }

    /// <summary>
    /// Value assigned e.g. for this.x = 5 -> '5
    /// </summary>
    public IExpression Value { get; init; }

    public SetExpression(IExpression Object, Token Name, IExpression Value)
    {
        this.Object = Object;
        this.Name = Name;
        this.Value = Value;
    }

    public object? EvaluateExpression()
    {
        object? result = Object.EvaluateExpression();
        if (result is KelloxInstance loxInstance)
        {
            object? value = Value.EvaluateExpression();
            loxInstance.Set(Name, value);
            return value;
        }
        throw new RunTimeError(Name, Messages.OnlyInstancesHaveFieldsErrorMessage);
    }

    public override string ToString()
    {
        object? result = Object.EvaluateExpression();
        return "set -> " + Name.Lexeme + "to " + (result is null ? Constants.NilKeyword : result.ToString()) + ".";
    }
}
