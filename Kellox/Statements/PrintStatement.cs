using Kellox.Classes;
using Kellox.Expressions;
using Kellox.Keywords;

namespace Kellox.Statements;

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
    /// Boolean value that determines weather a line break will be added after the printing the Expression - println
    /// </summary>
    private readonly bool addLineBreak;

    public PrintStatement(IExpression expression, bool addLineBreak)
    {
        this.Expression = expression;
        this.addLineBreak = addLineBreak;
    }

    public void Execute()
    {
        object? obj = Expression.Evaluate();
        switch (obj)
        {
            case null:
                Console.Write(KeywordConstants.NilKeyword);
                break;
            case bool logicalValue:
                Console.Write(logicalValue ? KeywordConstants.TrueKeyword : KeywordConstants.FalseKeyword);
                break;
            case double or string or KelloxInstance or int:
                Console.Write(obj);
                break;
            default:
                throw new NotImplementedException();
        }
        if (addLineBreak)
        {
            Console.Write(Environment.NewLine);
        }
    }
}
