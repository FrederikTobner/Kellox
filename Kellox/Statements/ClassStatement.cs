using Kellox.Classes;
using Kellox.Expressions;
using Kellox.Functions;
using Kellox.i18n;
using Kellox.Interpreter;
using Kellox.Tokens;
using Kellox.Utils;

namespace Kellox.Statements;

/// <summary>
/// Models a class statement in lox -> the whole class declaration
/// </summary>
internal class ClassStatement : IStatement
{
    /// <summary>
    /// Token that contains the class name
    /// </summary>
    public Token Token { get; init; }

    /// <summary>
    /// List of all the methods the class has
    /// </summary>
    public List<FunctionStatement> Methods { get; init; }

    /// <summary>
    /// The superClass of this class (nullable)
    /// </summary>
    public VariableExpression? SuperClass { get; init; }

    public ClassStatement(Token Name, List<FunctionStatement> Methods, VariableExpression? SuperClass)
    {
        this.Token = Name;
        this.Methods = Methods;
        this.SuperClass = SuperClass;
    }

    public void ExecuteStatement()
    {
        object? superClass = null;
        if (SuperClass is not null)
        {
            superClass = SuperClass.EvaluateExpression();
            // superClass has to be a Loxclass
            if (superClass is not KelloxClass)
            {
                ErrorLogger.Error(SuperClass.Token, Messages.SuperClassMustBeAClassErrorMessage);
            }
        }
        //Defines the class in the current environment
        KelloxInterpreter.currentEnvironment.Define(Token.Lexeme, null);
        if (superClass is not null)
        {
            KelloxInterpreter.currentEnvironment = new(KelloxInterpreter.currentEnvironment);
            KelloxInterpreter.currentEnvironment.Define(Constants.SuperKeyword, superClass);
        }
        Dictionary<string, KelloxFunction> newMethods = new();
        foreach (FunctionStatement? method in Methods)
        {
            if (method is not null)
            {
                newMethods.Add(method.Token.Lexeme, new KelloxFunction(method, KelloxInterpreter.currentEnvironment, method.Token.Lexeme.Equals(Constants.InitKeyword)));
            }
        }
        KelloxClass loxClass = new(Token.Lexeme, newMethods, (KelloxClass?)superClass);
        if (superClass is not null && KelloxInterpreter.currentEnvironment.Enclosing is not null)
        {
            KelloxInterpreter.currentEnvironment = KelloxInterpreter.currentEnvironment.Enclosing;
        }
        KelloxInterpreter.currentEnvironment.Assign(Token, loxClass);
    }
}
