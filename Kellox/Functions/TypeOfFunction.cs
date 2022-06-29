using Kellox.Classes;
using Kellox.Tokens;

namespace Kellox.Functions;

/// <summary>
/// Native typeof function -> used to determine the type of a variable in Kellox. Can be String, Number, Boolean, a kelloxclass or undefiened for nil
/// </summary>
internal class TypeOfFunction : IFunction
{
    public int Arity => 1;

    public object? Call(List<object?> arguments, Token paren) => arguments[0] switch
    {
        string => "String",
        double => "Number",
        bool => "Boolean",
        KelloxInstance kelloxInstance => kelloxInstance.KelloxClass.ToString(),
        null => "Undefiened",
        _ => throw new NotImplementedException()
    };
}
