using Interpreter.Exceptions;

namespace Interpreter
{
    /// <summary>
    /// Environment for the Programming Language
    /// Associates values to variables
    /// </summary>
    internal class CustomEnvironment
    {
        /// <summary>
        /// The enclosing Environment 
        /// e.g. 'global' if the Scope is definied in the global Scope
        /// </summary>
        private readonly CustomEnvironment? enclosing;

        /// <summary>
        /// Dictionary that conntains all the values defined in this Environment
        /// </summary>
        private readonly Dictionary<string, object?> values;

        public CustomEnvironment(CustomEnvironment? environment = null)
        {
            values = new Dictionary<string, object?>();
            this.enclosing = environment;
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
        /// <param name="name">The name of the variable</param>
        /// <returns>The value associated to the variable</returns>
        /// <exception cref="RunTimeError"></exception>
        public object? Get(Token name)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                return values[name.Lexeme];
            }
            if (enclosing != null)
            {
                return enclosing.values[name.Lexeme];
            }
            throw new RunTimeError(name, "Undefiened variable \'" + name.Lexeme + "\'.");
        }

        /// <summary>
        /// Assigns a value to a variable in the environment
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value that shall be assigned to the variable</param>
        /// <exception cref="RunTimeError"></exception>
        public void Assign(Token name, object? value)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                values[name.Lexeme] = value;
                return;
            }
            if (enclosing != null)
            {
                enclosing.Assign(name, value);
                return;
            }
            throw new RunTimeError(name, "Variable not defined yet. Assignment impossible");
        }
    }
}
