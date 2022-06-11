using Interpreter.Functions;

namespace Interpreter.Statements
{
    /// <summary>
    /// Models a class statement in lox -> the whole class declaration
    /// </summary>
    internal class ClassStatement : IStatement
    {

        /// <summary>
        /// Token that contains the class name
        /// </summary>
        public Token Name { get; init; }

        /// <summary>
        /// List of all the methods the class has
        /// </summary>
        public List<FunctionStatement> Methods { get; init; }

        public ClassStatement(Token Name, List<FunctionStatement> Methods)
        {
            this.Name = Name;
            this.Methods = Methods;
        }

        public void ExecuteStatement()
        {
            LoxInterpreter.currentEnvironment.Define(Name.Lexeme, null);
            Dictionary<string, LoxFunction> newMethods = new();
            foreach (FunctionStatement? method in Methods)
            {
                if (method is not null)
                {
                    newMethods.Add(method.Name.Lexeme, new LoxFunction(method, LoxInterpreter.currentEnvironment, method.Name.Lexeme.Equals("init")));
                }
            }
            LoxClass loxClass = new(Name.Lexeme, newMethods);
            LoxInterpreter.currentEnvironment.Assign(Name, loxClass);
        }
    }
}
