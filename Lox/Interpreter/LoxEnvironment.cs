using Lox.Interpreter.Exceptions;
using Lox.Tokens;

namespace Lox.Interpreter;

/// <summary>
/// Environment for the Programming Language
/// Associates values to variables
/// </summary>
internal class LoxEnvironment
{
    /// <summary>
    /// The enclosing Environment 
    /// e.g. 'global' if the Scope is definied in the global Scope
    /// </summary>
    public LoxEnvironment? Enclosing { get; set; }

    /// <summary>
    /// Dictionary that conntains all the values defined in this Environment
    /// </summary>
    private readonly Dictionary<string, object?> values;

    public LoxEnvironment(LoxEnvironment? environment = null)
    {
        values = new Dictionary<string, object?>();
        Enclosing = environment;
    }

    /// <summary>
    /// Assigns a value to a variable in the environment
    /// </summary>
    /// <param name="name">The name of the variable</param>
    /// <param name="value">The value that shall be assigned to the variable</param>
    /// <exception cref="RunTimeError">If the Variable is undefiened</exception>
    public void Assign(Token name, object? value)
    {
        if (values.ContainsKey(name.Lexeme))
        {
            values[name.Lexeme] = value;
            return;
        }
        if (Enclosing is not null)
        {
            Enclosing.Assign(name, value);
            return;
        }
        throw new RunTimeError(name, "Variable not defined yet. Assignment impossible");
    }

    /// <summary>
    /// Assign a value to a variable at the Environemt at a specified distance
    /// </summary>
    /// <param name="distance">distance to the specified environment ()from the current Environment</param>
    /// <param name="token">The Token (var identifierr)</param>
    /// <param name="value">The value assigned to the variable</param>
    public void AssignAt(int distance, Token token, object? value)
    {
        Ancestor(distance).values[token.Lexeme] = value;
    }

    /// <summary>
    /// Defines a new variable
    /// </summary>
    /// <param name="name">The name of the variable</param>
    /// <param name="value">The value that shall be assigned to the variable</param>
    public void Define(string name, object? value)
    {
        values.Add(name, value);
    }

    /// <summary>
    /// Gets the value associated to a specific Variable
    /// </summary>
    /// <param name="token">The identifier token</param>
    /// <returns>The value associated to the variable</returns>
    /// <exception cref="RunTimeError">If the Variable is undefiened</exception>
    public object? Get(Token token)
    {
        if (values.ContainsKey(token.Lexeme))
        {
            return values[token.Lexeme];
        }
        if (Enclosing is not null)
        {
            return Enclosing.Get(token);
        }
        throw new RunTimeError(token, "Undefiened variable \'" + token.Lexeme + "\'.");
    }

    /// <summary>
    /// Gets a value from the Environment at a specified distance
    /// </summary>
    /// <param name="distance">The specified distance</param>
    /// <param name="token">The IdentifierToken</param>
    /// <returns></returns>
    public object? GetAt(int distance, Token token)
    {
        return Ancestor(distance).Get(token);
    }

    /// <summary>
    /// Iterates through the ancestors/parents/enclosing environments of the current Environment
    /// </summary>
    /// <param name="distance"></param>
    private LoxEnvironment Ancestor(int distance)
    {
        LoxEnvironment environment = this;
        for (int i = 0; i < distance; i++)
        {
            if (environment.Enclosing is not null)
            {
                environment = environment.Enclosing;
            }
        }
        return environment;
    }
}
