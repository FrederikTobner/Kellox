﻿namespace Interpreter.Expressions
{
    internal class LiteralExpression : IExpression
    {
        public object? Value { get; init; }

        public LiteralExpression(object? value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Returns a representation of the Expression as a string
        /// </summary>
        public override string ToString()
        {
            if (Value is not null)
            {
                string? result = Value.ToString();
                if (result is not null)
                {
                    return result;
                }
            }
            return "nil";
        }

        public object? EvaluateExpression() => Value;
    }
}