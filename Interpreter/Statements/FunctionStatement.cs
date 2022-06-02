using Interpreter.Functions;

namespace Interpreter.Statements
{
    internal class FunctionStatement : IStatement
    {
        public Token Name { get; init; }

        public List<Token> Parameters { get; init; }

        public List<IStatement> Body { get; init; }

        public FunctionStatement(Token Name, List<Token> Parameters, List<IStatement> Body)
        {
            this.Name = Name;
            this.Parameters = Parameters;
            this.Body = Body;
        }

        public void ExecuteStatement()
        {
            LoxFunction loxFunction = new(this);
            LoxInterpreter.currentEnvironment.Define(Name.Lexeme, loxFunction);
        }
    }
}
