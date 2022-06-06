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
        public LoxClass Class { get; init; }

        private readonly Dictionary<string, object?> fields;

        public LoxInstance(LoxClass LoxClass)
        {
            this.Class = LoxClass;
            fields = new();
        }

        public override string ToString() => Class.ToString() + " instance";

        internal object? Get(Token name)
        {
            if (fields.ContainsKey(name.Lexeme))
            {
                return fields[name.Lexeme];
            }
            if (Class.Methods.ContainsKey(name.Lexeme))
            {
                return Class.Methods[name.Lexeme].Bind(this);
            }
            throw new RunTimeError(name, "Undefiened property \'" + name.Lexeme + "\'.");
        }

        internal void Set(Token name, object? value)
        {
            if (fields.ContainsKey(name.Lexeme))
            {
                fields[name.Lexeme] = value;
                return;
            }
            throw new RunTimeError(name, "Undefiened property \'" + name.Lexeme + "\'.");
        }
    }
}