namespace Kellox.Exceptions;

/// <summary>
/// Return Exception used for control flow
/// </summary>
internal class Return : ApplicationException
{
    /// <summary>
    /// The value that is returned
    /// </summary>
    public object? Value { get; init; }

    /// <summary>
    /// Constructor of the Return exception
    /// </summary>
    /// <param name="Value">Value that is returned</param>
    public Return(object? Value)
    {
        this.Value = Value;
    }
}
