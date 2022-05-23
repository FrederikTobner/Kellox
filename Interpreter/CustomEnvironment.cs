using Interpreter.Exceptions;

namespace Interpreter
{
    internal class CustomEnvironment
    {
        private readonly CustomEnvironment? enclosing;

        private readonly Dictionary<string, object?> values;

        public CustomEnvironment(CustomEnvironment? environment = null)
        {
            values = new Dictionary<string, object?>();
            this.enclosing = environment;
        }

        public void Define(string name, object? value)
        {
            values.Add(name, value);
        }

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
