namespace Interpreter
{
    /// <summary>
    /// The Type of a function, used to keep track whether the current scope is inside a function/method/initializer or no function at all
    /// </summary>
    public enum FUNCTIONTYPE
    {
        FUNCTION,
        INITIALIZER,
        METHOD,
        NONE
    }
}
