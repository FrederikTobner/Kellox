using Lox.Classes;
using Lox.Interpreter.Exceptions;
using Lox.Tokens;

namespace Lox.Expressions;

/// <summary>
/// Models a get expression in lox e.g. print rect.X -> I guess context is important
/// </summary>
internal class GetExpression : IExpression
{
    private const string OnlyIstancesHaveProbsErrorMessage = "Only Instances have properties";

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

    public object? EvaluateExpression()
    {
        object? result = Object.EvaluateExpression();
        if (result is LoxInstance loxInstance)
        {
            return loxInstance.Get(Name);
        }
        throw new RunTimeError(Name, OnlyIstancesHaveProbsErrorMessage);
    }

    public override string ToString() => Object.ToString() + " get -> " + Name.Lexeme;
}
