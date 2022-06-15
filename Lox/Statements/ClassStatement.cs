﻿using Lox.Classes;
using Lox.Expressions;
using Lox.Functions;
using Lox.Interpreter;
using Lox.LexicalAnalysis;
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
            if (superClass is not LoxClass)
            {
                LoxErrorLogger.Error(SuperClass.Token, "Superclass must be a class");
            }
        }
        LoxInterpreter.currentEnvironment.Define(Token.Lexeme, null);
        if (superClass is not null)
        {
            LoxInterpreter.currentEnvironment = new(LoxInterpreter.currentEnvironment);
            LoxInterpreter.currentEnvironment.Define("super", superClass);
        }
        Dictionary<string, LoxFunction> newMethods = new();
        foreach (FunctionStatement? method in Methods)
        {
            if (method is not null)
            {
                newMethods.Add(method.Name.Lexeme, new LoxFunction(method, LoxInterpreter.currentEnvironment, method.Name.Lexeme.Equals("init")));
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