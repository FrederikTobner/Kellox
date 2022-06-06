using Interpreter.Exceptions;

namespace Interpreter.Expressions
{
    internal class SetExpression : IExpression
    {
        public IExpression Object { get; init; }

        public Token Name { get; init; }

        public IExpression Value { get; init; }

        public SetExpression(IExpression Object, Token Name, IExpression Value)
        {
            this.Object = Object;
            this.Name = Name;
            this.Value = Value;
        }

        public object? EvaluateExpression()
        {
            object? result = Object.EvaluateExpression();
            if (result is LoxInstance loxInstance)
            {
                object? value = Value.EvaluateExpression();
                loxInstance.Set(Name, value);
                return value;
            }
            throw new RunTimeError(Name, "Only instances have fields");
        }

        public override string ToString()
        {
            object? result = Object.EvaluateExpression();
            return "set -> " + Name.Lexeme + "to " + (result is null ? "nil " : result.ToString());
        }
    }
}
