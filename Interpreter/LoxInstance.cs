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

        /// <summary>
        /// The fields of this Instance
        /// </summary>
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
            LoxFunction? method = Class.FindMethod(name.Lexeme);
            if (method is not null)
            {
                return method.Bind(this);
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
            fields.Add(name.Lexeme, value);
            return;
        }
    }
}