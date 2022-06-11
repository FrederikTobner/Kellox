using Lox.Functions;
using Lox.Interpreter;
using Lox.LexicalAnalysis;

namespace Lox.Statements;

/// <summary>
/// Models a function Statemant (Function Declaration)
/// </summary>
internal class FunctionStatement : IStatement
{
    /// <summary>
    /// The Token of the Name of the function
    /// </summary>
    public Token Name { get; init; }

    /// <summary>
    /// The parameters of the function
    /// </summary>
    public List<Token> Parameters { get; init; }

    /// <summary>
    /// The Function Body
    /// </summary>
    public List<IStatement> Body { get; init; }

    /// <summary>
    /// The Constructor if a FunctionStatement
    /// </summary>
    /// <param name="Name">The Name of the Function</param>
    /// <param name="Parameters">The parameters that the function expects</param>
    /// <param name="Body">The statements in the body of the Function</param>
    public FunctionStatement(Token Name, List<Token> Parameters, List<IStatement> Body)
    {
        this.Name = Name;
        this.Parameters = Parameters;
        this.Body = Body;
    }

    public void ExecuteStatement()
    {
        LoxFunction loxFunction = new(this, LoxInterpreter.currentEnvironment, false);
        LoxInterpreter.currentEnvironment.Define(Name.Lexeme, loxFunction);
    }
}
