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
            object? result = null;
            // Saves the old environment
            LoxEnvironment oldEnvironment = LoxInterpreter.currentEnvironment;
            // Creates a new Environemt for the scope of the function body
            LoxInterpreter.currentEnvironment = new(Closure);
            for (int i = 0; i < Declaration.Parameters.Count; i++)
            {
                LoxInterpreter.currentEnvironment.Define(Declaration.Parameters[i].Lexeme, arguments[i]);
            }
            //Catches Return exception and returns value
            try
            {
                foreach (IStatement? statement in Declaration.Body)
                {
                    statement.ExecuteStatement();
                }
            }
            // Catches return
            catch (Return returnValue)
            {
                result = returnValue.Value;
            }
            //Resets Environment
            LoxInterpreter.currentEnvironment = oldEnvironment;
            return result;
        }

        public override string ToString() => "<fn " + Declaration.Name.Lexeme + ">";
    }
}
