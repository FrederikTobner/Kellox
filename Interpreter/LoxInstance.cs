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

        public LoxInstance(LoxClass LoxClass)
        {
            this.LoxClass = LoxClass;
        }

        public override string ToString() => LoxClass.ToString() + " instance";
    }
}