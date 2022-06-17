using Lox.Expressions;
using System.Text;

namespace Lox.Statements;

/// <summary>
/// Models a print statement
/// </summary>
internal class PrintStatement : IStatement
{
    /// <summary>
    /// The Expression that shall be printed e.g "Hallo" / 6.3
    /// </summary>
    public IExpression Expression { get; init; }

    /// <summary>
    /// Boolean value that determines weather a line break will be added after the printing the Expression
    /// </summary>
    private readonly bool addLineBreak;

    public PrintStatement(IExpression expression, bool addLineBreak = false)
    {
        this.addLineBreak = addLineBreak;
        this.Expression = expression;
    }

    public void ExecuteStatement()
    {
        object? obj = Expression.EvaluateExpression();
        if (obj is not null)
        {
            if (obj is string text)
            {
                StringBuilder stringBuilder = new(text);
                stringBuilder.Replace("\\n", "\n");
                Console.Write(stringBuilder.ToString());
            }
            else
            {
                Console.Write(obj);
            }


        }
        else
        {
            Console.Write("nil");
        }
        if (addLineBreak)
        {
            Console.Write(Environment.NewLine);
        }
    }
}
