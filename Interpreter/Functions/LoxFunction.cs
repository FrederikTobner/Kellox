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

        /// <summary>
        /// The arity of the function -> fancy term for the number of arguments a function expects
        /// </summary>
        public int Arity => this.Declaration.Parameters.Count;

        public object? Call(List<object?> arguments)
        {
            LoxEnvironment environment = new(Closure);
            for (int i = 0; i < Declaration.Parameters.Count; i++)
            {
                environment.Define(Declaration.Parameters[i].Lexeme, arguments[i]);
            }
            LoxEnvironment oldEnvironment = LoxInterpreter.currentEnvironment;
            LoxInterpreter.currentEnvironment = environment;
            //Catches Return and returns value
            try
            {
                new BlockStatement(Declaration.Body).ExecuteStatement();
            }
            catch (Return returnValue)
            {
                return returnValue.Value;
            }
            LoxInterpreter.currentEnvironment = oldEnvironment;
            return null;
        }

        public override string ToString() => "<fn " + Declaration.Name.Lexeme + ">";
    }
}
