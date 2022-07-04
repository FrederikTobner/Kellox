using Kellox.Classes;
using Kellox.Exceptions;
using Kellox.Tokens;

namespace Kellox.Expressions;

/// <summary>
/// Models a get expression in lox e.g. print rect.X -> I guess context is important
/// </summary>
internal class GetExpression : IExpression
{
    /// <summary>
    /// Object of the Expression rect.x -> 'rect'
    /// </summary>
    public IExpression Object { get; init; }

    /// <summary>
    /// Name of the Property that is accessed e.g. rect.x -> 'x'
    /// </summary>
    public Token Name { get; init; }

    public GetExpression(IExpression Object, Token Name)
    {
        this.Object = Object;
        this.Name = Name;
    }

    public object? Evaluate()
    {
        object? result = Object.Evaluate();
        if (result is KelloxInstance loxInstance)
        {
            return loxInstance.Get(Name);
        }
        throw new RunTimeError(Name, "Only Instances have properties");
    }

    public override string ToString() => Object.ToString() + " get -> " + Name.Lexeme;
}
