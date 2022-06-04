using Interpreter.Exceptions;
using Interpreter.Functions;

namespace Interpreter.Expressions
{
    internal class CallExpression : IExpression
    {
        public IExpression Calle { get; init; }

        public Token Paren { get; init; }

        public IReadOnlyList<IExpression> Arguments { get; init; }

        public CallExpression(IExpression calle, Token paren, List<IExpression> arguments)
        {
            this.Calle = calle;
            this.Paren = paren;
            this.Arguments = arguments;
        }

        public object? EvaluateExpression()
        {
            object? callee = Calle.EvaluateExpression();

            List<object?> arguments = new();

            foreach (IExpression expression in Arguments)
            {
                arguments.Add(expression.EvaluateExpression());
            }
            if (callee is IFunction function)
            {
                if (arguments.Count != function.Arity)
                {
                    throw new RunTimeError(this.Paren, "Expected " + function.Arity + " argumnets but got" + arguments.Count + ".");
                }
                return function?.Call(arguments);
            }
            throw new RunTimeError(this.Paren, "Can only call functions and classes");
        }

        public override string ToString() => IExpression.Parenthesize(this.Calle.ToString(), this.Arguments.ToArray());
    }
}
