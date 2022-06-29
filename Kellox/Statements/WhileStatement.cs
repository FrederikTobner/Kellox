using Kellox.Exceptions;
using Kellox.Expressions;
using Kellox.Interpreter;

namespace Kellox.Statements;

/// <summary>
/// Models a While Statement in lox
/// </summary>
internal class WhileStatement : IStatement
{
    /// <summary>
    /// The Condition of the WhileStatement e.g. true / a < 6
    /// </summary>
    public IExpression Condition { get; init; }

    /// <summary>
    /// The body of the whileStatement e.g. a Block//PrintStatement
    /// </summary>
    public IStatement Body { get; init; }

    public WhileStatement(IExpression expression, IStatement body)
    {
        this.Condition = expression;
        this.Body = body;
    }

    public void Execute()
    {
        KelloxEnvironment kelloxEnvironment = KelloxInterpreter.currentEnvironment;
        while (IExpression.IsTruthy(Condition.Evaluate()))
        {
            try
            {
                Body.Execute();
            }
            catch(Continue)
            {
                KelloxInterpreter.currentEnvironment = kelloxEnvironment;
            }
            catch (Break)
            {
                KelloxInterpreter.currentEnvironment = kelloxEnvironment;
                break;
            }
        }
    }
}
