using Lox.Classes;
using Lox.Expressions;
using Lox.Functions;
using Lox.Interpreter;
using Lox.Messages;
using Lox.Tokens;
using Lox.Utils;

namespace Lox.Statements;

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
            if (superClass is not LoxClass)
            {
                LoxErrorLogger.Error(SuperClass.Token, MessageUtils.SuperClassMustBeAClassErrorMessage);
            }
        }
        //Defines the class in the current environment
        LoxInterpreter.currentEnvironment.Define(Token.Lexeme, null);
        if (superClass is not null)
        {
            LoxInterpreter.currentEnvironment = new(LoxInterpreter.currentEnvironment);
            LoxInterpreter.currentEnvironment.Define(MessageUtils.SuperKeyword, superClass);
        }
        Dictionary<string, LoxFunction> newMethods = new();
        foreach (FunctionStatement? method in Methods)
        {
            if (method is not null)
            {
                newMethods.Add(method.Token.Lexeme, new LoxFunction(method, LoxInterpreter.currentEnvironment, method.Token.Lexeme.Equals(MessageUtils.InitKeyword)));
            }
        }
        LoxClass loxClass = new(Token.Lexeme, newMethods, (LoxClass?)superClass);
        if (superClass is not null && LoxInterpreter.currentEnvironment.Enclosing is not null)
        {
            LoxInterpreter.currentEnvironment = LoxInterpreter.currentEnvironment.Enclosing;
        }
        LoxInterpreter.currentEnvironment.Assign(Token, loxClass);
    }
}
