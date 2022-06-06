using Interpreter.Functions;

namespace Interpreter.Statements
{
    internal class ClassStatement : IStatement
    {
        public Token Name { get; init; }

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
                    newMethods.Add(method.Name.Lexeme, new LoxFunction(method, LoxInterpreter.currentEnvironment));
                }
            }
            LoxClass loxClass = new(Name.Lexeme, newMethods);
            LoxInterpreter.currentEnvironment.Assign(Name, loxClass);
        }
    }
}
