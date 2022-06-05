using Interpreter.Exceptions;
using Interpreter.Statements;

namespace Interpreter.Functions
{
    internal class LoxFunction : IFunction
    {
        /// <summary>
        /// The function statement where the function is defined
        /// </summary>
        public FunctionStatement Declaration { get; init; }

        /// <summary>
        /// Data structure that closes over and holds on to the surrounding variables where the function is declared
        /// </summary>
        public LoxEnvironment Closure { get; init; }

        public LoxFunction(FunctionStatement Declaration, LoxEnvironment Closure)
        {
            this.Declaration = Declaration;
            this.Closure = Closure;
        }

        public int Arity => this.Declaration.Parameters.Count;

        public object? Call(List<object?> arguments)
        {
            LoxEnvironment environment = new(Closure);
            for (int i = 0; i < Declaration.Parameters.Count; i++)
            {
                environment.Define(Declaration.Parameters[i].Lexeme, arguments[i]);
            }
            LoxInterpreter.currentEnvironment = environment;
            //Catches Return and returns value
            try
            {
                new BlockStatement(Declaration.Body).ExecuteStatement();
            }
            catch (Return returnValue)
            {
                LoxInterpreter.currentEnvironment = Closure;
                return returnValue.Value;
            }
            LoxInterpreter.currentEnvironment = Closure;
            return null;
        }

        public override string ToString() => "<fn " + Declaration.Name.Lexeme + ">";
    }
}
