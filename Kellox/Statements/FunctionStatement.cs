using Lox.Functions;
using Lox.Interpreter;
using Lox.Tokens;

namespace Lox.Statements;

/// <summary>
/// Models a function Statemant (Function Declaration)
/// </summary>
internal class FunctionStatement : IStatement
{
    /// <summary>
    /// The Token of the Name of the function
    /// </summary>
    public Token Token { get; init; }

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
    /// <param name="token">The Name of the Function</param>
    /// <param name="Parameters">The parameters that the function expects</param>
    /// <param name="Body">The statements in the body of the Function</param>
    public FunctionStatement(Token token, List<Token> Parameters, List<IStatement> Body)
    {
        this.Token = token;
        this.Parameters = Parameters;
        this.Body = Body;
    }

    public void ExecuteStatement()
    {
        KelloxFunction loxFunction = new(this, LoxInterpreter.currentEnvironment, false);
        LoxInterpreter.currentEnvironment.Define(Token.Lexeme, loxFunction);
    }
}
