using Interpreter.Exceptions;

namespace Interpreter
{
    internal class CustomEnvironment
    {
        private readonly Dictionary<string, object> values;

        public CustomEnvironment()
        {
            values = new Dictionary<string, object>();
        }

        public void Define(string name, object? value)
        {
            values.Add(name, value);
        }

        public object Get(Token name)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                return values[name.Lexeme];
            }
            throw new RunTimeError(name, "Undefiened variable \'" + name.Lexeme + "\'.");
        }

        public void Assign(Token name, object value)
        {
            if (values.ContainsKey(name.Lexeme))
            {
                values[name.Lexeme] = value;
            }
        }
    }
}
