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
            LoxEnvironment oldEnvironment = LoxInterpreter.currentEnvironment;
            LoxInterpreter.currentEnvironment = new(Closure);
            for (int i = 0; i < Declaration.Parameters.Count; i++)
            {
                LoxInterpreter.currentEnvironment.Define(Declaration.Parameters[i].Lexeme, arguments[i]);
            }
            //Catches Return and returns value
            try
            {
                new BlockStatement(Declaration.Body).ExecuteStatement();
            }
            catch (Return returnValue)
            {
                LoxInterpreter.currentEnvironment = oldEnvironment;
                return returnValue.Value;
            }
            LoxInterpreter.currentEnvironment = oldEnvironment;
            return null;
        }

        public override string ToString() => "<fn " + Declaration.Name.Lexeme + ">";
    }
}
