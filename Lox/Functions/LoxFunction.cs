using Lox.Classes;
using Lox.Functions.Exceptions;
using Lox.Interpreter;
using Lox.LexicalAnalysis;
using Lox.Statements;

namespace Lox.Functions;

internal class LoxFunction : IFunction
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
    public LoxEnvironment Closure { get; init; }

    public LoxFunction(FunctionStatement Declaration, LoxEnvironment Closure, bool isInitializer)
    {
        this.Declaration = Declaration;
        this.Closure = Closure;
        this.IsInitializer = isInitializer;
    }

    public int Arity => this.Declaration.Parameters.Count;

    public object? Call(List<object?> arguments)
    {
        object? result = null;
        // Saves the old environment
        LoxEnvironment oldEnvironment = LoxInterpreter.currentEnvironment;
        // Creates a new Environemt for the scope of the function body
        LoxInterpreter.currentEnvironment = new(Closure);
        for (int i = 0; i < Declaration.Parameters.Count; i++)
        {
            LoxInterpreter.currentEnvironment.Define(Declaration.Parameters[i].Lexeme, arguments[i]);
        }
        //Catches Return exception and returns value
        try
        {
            foreach (IStatement? statement in Declaration.Body)
            {
                statement.ExecuteStatement();
            }
        }
        // Catches return
        catch (Return returnValue)
        {
            result = returnValue.Value;
        }
        if (IsInitializer)
        {
            //Calls the constructor/initializer
            result = Closure.GetAt(0, new Token(TokenType.THIS, "this", null, 0));
        }
        //Resets Environment
        LoxInterpreter.currentEnvironment = oldEnvironment;
        return result;
    }

    internal LoxFunction Bind(LoxInstance loxInstance)
    {
        LoxEnvironment environment = new(Closure);
        environment.Define("this", loxInstance);
        return new(Declaration, environment, IsInitializer);
    }

    public override string ToString() => "<fun " + Declaration.Name.Lexeme + ">";
}
