using System.Runtime.CompilerServices;

namespace SGuard.Exceptions;

/// <summary>
/// Represents an exception that occurs when a value falls within the specified range.
/// </summary>
[Serializable]
public sealed class BetweenException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BetweenException"/> class.
    /// </summary>
    public BetweenException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="BetweenException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public BetweenException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="BetweenException"/> class with a specified error message 
    /// and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public BetweenException(string message, Exception inner) : base(message, inner) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="BetweenException"/> class building its own message from arguments.
    /// CallerArgumentExpression captures the real call-site expressions.
    /// </summary>
    public BetweenException(
        object? value,
        object? min,
        object? max,
        [CallerArgumentExpression("value")] string? valueExpr = null,
        [CallerArgumentExpression("min")]   string? minExpr   = null,
        [CallerArgumentExpression("max")]   string? maxExpr   = null)
        : base(BuildMessage(value, min, max, valueExpr, minExpr, maxExpr))
    {
        Data["value"]     = value;
        Data["min"]       = min;
        Data["max"]       = max;
        Data["valueExpr"] = valueExpr;
        Data["minExpr"]   = minExpr;
        Data["maxExpr"]   = maxExpr;
    }

    private static string BuildMessage(
        object? value, object? min, object? max,
        string? valueExpr, string? minExpr, string? maxExpr)
    {
        return $"Value '{valueExpr}' is between '{minExpr}' and '{maxExpr}'. " +
               $"Actual: value={value}, min={min}, max={max}.";
    }

}