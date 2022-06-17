using Lox.Functions;
using Lox.Interpreter.Exceptions;
using Lox.Tokens;
using Lox.Utils;
using System.Text;

namespace Lox.Classes;

/// <summary>
/// Models a specific Instance of a LoxClass
/// </summary>
internal class KelloxInstance
{
    /// <summary>
    /// The class of this Instance
    /// </summary>
    public KelloxClass Class { get; init; }

    /// <summary>
    /// The fields of this Instance
    /// </summary>
    private readonly Dictionary<string, object?> fields;

    public KelloxInstance(KelloxClass LoxClass)
    {
        Class = LoxClass;
        fields = new();
    }

    public override string ToString() => KelloxInstanceSerializer.Serialize(this.fields);

    /// <summary>
    /// Used to get a method//property associated with the instance
    /// </summary>
    /// <param name="name">The Token of the property/method</param>
    /// <exception cref="RunTimeError"></exception>
    internal object? Get(Token name)
    {
        if (fields.ContainsKey(name.Lexeme))
        {
            return fields[name.Lexeme];
        }
        KelloxFunction? method = Class.FindMethod(name.Lexeme);
        if (method is not null)
        {
            return method.Bind(this);
        }
        throw new RunTimeError(name, "Undefiened property \'" + name.Lexeme + "\'.");
    }

    /// <summary>
    /// Used to set a property the value of a property
    /// </summary>
    /// <param name="name">The Token of the property</param>
    /// <param name="value">The value that is assigned to the property</param>
    internal void Set(Token name, object? value)
    {
        if (fields.ContainsKey(name.Lexeme))
        {
            fields[name.Lexeme] = value;
            return;
        }
        fields.Add(name.Lexeme, value);
        return;
    }
}
