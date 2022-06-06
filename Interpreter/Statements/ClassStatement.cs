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
            LoxClass loxClass = new(Name.Lexeme);
            LoxInterpreter.currentEnvironment.Assign(Name, loxClass);
        }
    }
}
