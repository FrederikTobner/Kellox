using Kellox.Expressions;
using Kellox.Keywords;
using Kellox.Tokens;
using Kellox.Utils;

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
    /// Boolean value that determines weather a line break will be added after the printing the Expression
    /// </summary>
    private readonly bool addLineBreak;

    /// <summary>
    /// Token of the print statement used for error logging
    /// </summary>
    private readonly Token printToken;

    public PrintStatement(IExpression expression, Token printToken, bool addLineBreak)
    {
        this.Expression = expression;
        this.printToken = printToken;
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
            case string text:
                Console.Write(text);
                break;
            case bool logicalValue:
                Console.Write(logicalValue ? KeywordConstants.TrueKeyword : KeywordConstants.FalseKeyword);
                break;
            case object:
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
