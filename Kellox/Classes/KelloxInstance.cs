using Kellox.Exceptions;
using Kellox.Functions;
using Kellox.Tokens;
using Kellox.Utils;
using System.Text.Json.Serialization;

namespace Kellox.Classes;

/// <summary>
/// Models a specific Instance of a LoxClass
/// </summary>
internal class KelloxInstance
{
    /// <summary>
    /// The class of this Instance
    /// </summary>
    public KelloxClass KelloxClass { get; init; }

    /// <summary>
    /// The fields of this Instance
    /// </summary>
    private readonly Dictionary<string, object?> fields;

    /// <summary>
    /// Creates a new Kellox instance 🚼
    /// </summary>
    /// <param name="kelloxClass">The class of the instance</param>
    public KelloxInstance(KelloxClass kelloxClass)
    {
        this.KelloxClass = kelloxClass;
        this.fields = new();
    }

    /// <summary>
    /// Converts this KelloxInstace to a JSON string
    /// </summary>
    public override string ToString() => KelloxInstanceSerializer.Serialize(this.fields);

    /// <summary>
    /// Used to get a method//property associated with the instance
    /// </summary>
    /// <param name="name">The Token of the property/method</param>
    /// <exception cref="RunTimeError">Throws an runtime error if the property is not defined</exception>
    internal object? Get(Token name)
    {
        if (this.fields.ContainsKey(name.Lexeme))
        {
            return this.fields[name.Lexeme];
        }
        if (this.KelloxClass.TryFindMethod(name.Lexeme, out KelloxFunction? method))
        {
            return method?.Bind(this);
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
        if (this.fields.ContainsKey(name.Lexeme))
        {
            this.fields[name.Lexeme] = value;
            return;
        }
        this.fields.Add(name.Lexeme, value);
        return;
    }
}
