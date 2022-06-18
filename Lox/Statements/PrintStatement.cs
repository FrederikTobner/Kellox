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
            if (obj is string stringObject)
            {                
                StringBuilder stringBuilder = new(stringObject);
                // Adds a linebreak for every '\n' in the string
                stringBuilder.Replace("\\n", Environment.NewLine);
                Console.Write(stringBuilder.ToString());
            }
            else
            {
                // prints out a double or boolean value
                Console.Write(obj);
            }

        }
        else
        {
            // Prints out a null value -> nil in lox
            Console.Write("nil");
        }
        // Adds a linebreak if the statement is a println statement
        if (addLineBreak)
        {
            Console.Write(Environment.NewLine);
        }
    }
}
