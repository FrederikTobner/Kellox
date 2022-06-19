using Kellox.Interpreter;
using Kellox.Tokens;

namespace Kellox.Expressions
{
    /// <summary>
    /// Models a Variable expression in lox
    /// </summary>
    internal class VariableExpression : IExpression
    {
        /// <summary>
        /// The Token of the Variable expression (the variable name/identifier)
        /// </summary>
        public Token Token { get; init; }

        public VariableExpression(Token token)
        {
            this.Token = token;
        }

        public object? Evaluate()
        {
            if (KelloxInterpreter.TryGetDepthOfLocal(this, out int distance))
            {
                return KelloxInterpreter.currentEnvironment.GetAt(distance, Token);
            }
            else
            {
                return KelloxInterpreter.globalEnvironment.Get(Token);
            }
        }

        /// <summary>
        /// Returns a representation of the Expression as a string
        /// </summary>
        public override string ToString() => $"Var ({Token.Lexeme})";
    }
}
