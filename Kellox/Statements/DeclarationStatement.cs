using Kellox.Expressions;
using Kellox.Interpreter;
using Kellox.Tokens;

namespace Kellox.Statements;

/// <summary>
/// Models a declaration statement
/// </summary>
internal class DeclarationStatement : IStatement
{
    /// <summary>
    /// The Identifier Token of this declaration statement
    /// </summary>
    public Token Name { get; init; }

    /// <summary>
    /// The expression that evaluated and assigned to the variable
    /// </summary>
    public IExpression? Expression { get; init; }

    /// <summary>
    /// The Cunstructor of the DeclarationStatement class
    /// </summary>
    /// <param name="name">The Token of the variable name that is declared</param>
    /// <param name="expression">The value that is assigned to the variable, null by default</param>
    public DeclarationStatement(Token name, IExpression? expression = null)
    {
        this.Name = name;
        this.Expression = expression;
    }

    public void ExecuteStatement()
    {
        object? value = null;
        if (Expression is not null)
        {
            value = Expression.EvaluateExpression();
        }
        LoxInterpreter.currentEnvironment.Define(Name.Lexeme, value);
    }
}
