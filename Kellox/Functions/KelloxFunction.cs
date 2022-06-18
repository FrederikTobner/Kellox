using Kellox.Classes;
using Kellox.Exceptions;
using Kellox.Interpreter;
using Kellox.Statements;
using Kellox.Tokens;

namespace Kellox.Functions;

internal class KelloxFunction : IFunction
{
    /// <summary>
    /// Boolean value that determines whether the function is a constructor/initializer
    /// </summary>
    private bool IsInitializer { get; init; }

    /// <summary>
    /// The function statement where the function is defined
    /// </summary>
    public FunctionStatement Declaration { get; init; }

    /// <summary>
    /// Data structure that closes over and holds on to the surrounding variables where the function is declared
    /// </summary>
    public KelloxEnvironment Closure { get; init; }

    public KelloxFunction(FunctionStatement Declaration, KelloxEnvironment Closure, bool isInitializer)
    {
        this.Declaration = Declaration;
        this.Closure = Closure;
        this.IsInitializer = isInitializer;
    }

    public int Arity => this.Declaration.Parameters.Count;

    public object? Call(List<object?> arguments, Token paren)
    {
        object? result = null;
        // Saves the old environment
        KelloxEnvironment oldEnvironment = KelloxInterpreter.currentEnvironment;
        // Creates a new Environemt for the scope of the function body
        KelloxInterpreter.currentEnvironment = new(Closure);
        for (int i = 0; i < Declaration.Parameters.Count; i++)
        {
            KelloxInterpreter.currentEnvironment.Define(Declaration.Parameters[i].Lexeme, arguments[i]);
        }
        //Catches Return exception and returns value
        try
        {
            foreach (IStatement? statement in Declaration.Body)
            {
                statement.ExecuteStatement();
            }
        }
        catch (Return returnValue)
        {
            result = returnValue.Value;
        }
        if (IsInitializer)
        {
            //Calls the constructor/initializer
            result = Closure.GetAt(0, new Token(TokenType.THIS, "this", null, Declaration.Token.Line));
        }
        //Resets Environment
        KelloxInterpreter.currentEnvironment = oldEnvironment;
        return result;
    }

    // Binds an instance to the enviroment of the function
    internal KelloxFunction Bind(KelloxInstance loxInstance)
    {
        KelloxEnvironment environment = new(Closure);
        environment.Define("this", loxInstance);
        return new(Declaration, environment, IsInitializer);
    }

    public override string ToString() => "<function " + Declaration.Token.Lexeme + ">";
}
