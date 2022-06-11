namespace Lox.Resolver;

/// <summary>
/// The classtype used to keep track whether the current scope is inside a class, a subclass or no class at all
/// </summary>
internal enum ClassType
{
    CLASS,
    NONE,
    SUBCLASS
}
