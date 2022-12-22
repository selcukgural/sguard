namespace SGuard.Option;

/// <summary>
/// Represents a callback option that can be used to specify a callback action to be invoked when a condition is not met.
/// </summary>
public class CallbackOption
{
    private Action? _callbackWithoutParameter;
    private Action<string>? _callbackWithParameter;

    /// <summary>
    /// Gets or sets a value indicating whether the callback should be invoked when a null value is encountered.
    /// </summary>
    public bool IsNullThrowFailure { get; set; }

    /// <summary>
    /// Sets the callback action to be invoked without a parameter.
    /// </summary>
    /// <param name="callback">The callback action to be invoked without a parameter.</param>
    public void SetCallback(Action? callback)
    {
        _callbackWithoutParameter = callback;
        IsNullThrowFailure = true;
    }

    /// <summary>
    /// Sets the callback action to be invoked with a string parameter.
    /// </summary>
    /// <param name="callback">The callback action to be invoked with a string parameter.</param>
    public void SetCallback(Action<string>? callback)
    {
        _callbackWithParameter = callback;
        IsNullThrowFailure = true;
    }

    /// <summary>
    /// Invokes the callback action.
    /// </summary>
    internal void InvokeCallback()
    {
        _callbackWithoutParameter?.Invoke();
        _callbackWithParameter?.Invoke(string.Empty);
    }

    /// <summary>
    /// Determines whether a callback action has been set.
    /// </summary>
    /// <returns>True if a callback action has been set, false otherwise.</returns>
    internal bool HasCallback()
    {
        return _callbackWithoutParameter != null || _callbackWithParameter != null;
    }
}