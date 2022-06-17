namespace Kellox.Resolver;

/// <summary>
/// The Type of a function, used to keep track whether the current scope is inside a function/method/initializer or no function at all
/// </summary>
public enum FunctionType
{
    FUNCTION,
    INITIALIZER,
    METHOD,
    NONE
}
