using Interpreter.Exceptions;

namespace Interpreter.Expressions
{
    internal class GetExpression : IExpression
    {
        public IExpression Object { get; init; }

        public Token Name { get; init; }

        public GetExpression(IExpression Object, Token Name)
        {
            this.Object = Object;
            this.Name = Name;
        }

        public object? EvaluateExpression()
        {
            object? result = Object.EvaluateExpression();
            if (result is LoxInstance loxInstance)
            {
                return loxInstance.Get(Name);
            }
            throw new RunTimeError(Name, "Only Instances have properties");
        }

        public override string ToString() => "get" + Name.Lexeme;
    }
}
