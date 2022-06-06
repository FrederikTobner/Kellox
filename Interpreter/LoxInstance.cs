using Interpreter.Exceptions;
using Interpreter.Functions;

namespace Interpreter
{
    /// <summary>
    /// Models a specific Instance of a LoxClass
    /// </summary>
    internal class LoxInstance
    {
        /// <summary>
        /// The class of this Instance
        /// </summary>
        public LoxClass LoxClass { get; init; }

        private readonly Dictionary<string, object?> fields;

        public LoxInstance(LoxClass LoxClass)
        {
            this.LoxClass = LoxClass;
            fields = new();
        }

        public override string ToString() => LoxClass.ToString() + " instance";

        internal object? Get(Token name)
        {
            if (fields.ContainsKey(name.Lexeme))
            {
                return fields[name.Lexeme];
            }
            if (LoxClass.Methods.ContainsKey(name.Lexeme))
            {
                return LoxClass.Methods[name.Lexeme];
            }
            throw new RunTimeError(name, "Undefiened property \'" + name.Lexeme + "\'.");
        }

        internal void Set(Token name, object? value)
        {
            if (!fields.ContainsKey(name.Lexeme))
            {
                fields.Add(name.Lexeme, value);
            }
            else
            {
                fields[name.Lexeme] = value;
            }
        }
    }
}