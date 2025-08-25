using System.Diagnostics.CodeAnalysis;

namespace SGuard;

/// <summary>
/// The <see cref="ThrowIf"/> class provides static methods for validating
/// and asserting conditions to ensure data integrity. It offers methods to
/// throw exceptions when certain conditions are met.
/// </summary>
public sealed partial class ThrowIf
{
    /// <summary>
    /// Throws an exception if the specified value is between the provided minimum and maximum values.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to evaluate.</typeparam>
    /// <typeparam name="TMin">The type of the minimum value.</typeparam>
    /// <typeparam name="TMax">The type of the maximum value.</typeparam>
    /// <param name="value">The value to evaluate.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="callback">An optional callback that will be invoked with the guard evaluation outcome.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/>, <paramref name="min"/>, or <paramref name="max"/> is null.</exception>
    public static void Between<TValue, TMin, TMax>([NotNull] TValue value, [NotNull] TMin min, [NotNull] TMax max, SGuardCallback? callback = null)
        where TValue : IComparable<TMin>, IComparable<TMax>
    {
        var isBetween = false;

        try
        {
            ArgumentNullException.ThrowIfNull(min);
            ArgumentNullException.ThrowIfNull(max);
            ArgumentNullException.ThrowIfNull(value);
            
            isBetween = Is.Between(value, min, max);

            if (isBetween)
            {
                Throw.BetweenException(value, min, max);
            }
        }
        finally
        {
            callback?.Invoke(isBetween ? GuardOutcome.Failure : GuardOutcome.Success);
        }
    }

    /// <summary>
    /// Throws an exception if the specified value is between the given minimum and maximum bounds.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to evaluate.</typeparam>
    /// <typeparam name="TMin">The type of the minimum bound.</typeparam>
    /// <typeparam name="TMax">The type of the maximum bound.</typeparam>
    /// <typeparam name="TException">The type of the exception to throw if the value is between the bounds.</typeparam>
    /// <param name="value">The value to evaluate.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="exception">The exception to be thrown if the condition is met.</param>
    /// <param name="callback">An optional callback invoked with the outcome of the evaluation.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/>, <paramref name="min"/>, <paramref name="max"/>, or <paramref name="exception"/> is null.</exception>
    /// <exception cref="TException">Thrown if <paramref name="value"/> is between <paramref name="min"/> and <paramref name="max"/>.</exception>
    public static void Between<TValue, TMin, TMax, TException>([NotNull] TValue value, [NotNull] TMin min, [NotNull] TMax max,
                                                               [NotNull] TException exception, SGuardCallback? callback = null)
        where TValue : IComparable<TMin>, IComparable<TMax> where TException : Exception
    {
        var isBetween = false;
        
        try
        {
            ArgumentNullException.ThrowIfNull(min);
            ArgumentNullException.ThrowIfNull(max);
            ArgumentNullException.ThrowIfNull(value);
            ArgumentNullException.ThrowIfNull(exception);

            isBetween = Is.Between(value, min, max);

            if (isBetween)
            {
                Throw.That(exception);
            }
        }
        finally
        {
            callback?.Invoke(isBetween ? GuardOutcome.Failure : GuardOutcome.Success);
        }
    }
    
    /// <summary>
    /// Throws an exception if the specified string is between the provided bounds using the given StringComparison.
    /// </summary>
    /// <param name="value">The string value to evaluate.</param>
    /// <param name="min">The minimum bound (inclusive).</param>
    /// <param name="max">The maximum bound (inclusive).</param>
    /// <param name="comparison">The string comparison rule to use.</param>
    /// <param name="callback">Optional guard outcome callback.</param>
    public static void Between([NotNull] string value, [NotNull] string min, [NotNull] string max,
                               StringComparison comparison, SGuardCallback? callback = null)
    {
        var isBetween = false;

        try
        {
            ArgumentNullException.ThrowIfNull(min);
            ArgumentNullException.ThrowIfNull(max);
            ArgumentNullException.ThrowIfNull(value);

            isBetween = Is.Between(value, min, max, comparison);

            if (isBetween)
            {
                Throw.BetweenException(value, min, max);
            }
        }
        finally
        {
            callback?.Invoke(isBetween ? GuardOutcome.Failure : GuardOutcome.Success);
        }
    }
    
    /// <summary>
    /// Throws the provided exception if the specified string is between the given bounds using the given StringComparison.
    /// </summary>
    /// <param name="value">The string value to evaluate.</param>
    /// <param name="min">The minimum bound (inclusive).</param>
    /// <param name="max">The maximum bound (inclusive).</param>
    /// <param name="comparison">The string comparison rule to use.</param>
    /// <param name="exception">The exception to throw when the condition is met.</param>
    /// <param name="callback">Optional guard outcome callback.</param>
    public static void Between<TException>([NotNull] string value, [NotNull] string min, [NotNull] string max,
                                           StringComparison comparison, [NotNull] TException exception,
                                           SGuardCallback? callback = null) where TException : Exception
    {
        var isBetween = false;

        try
        {
            ArgumentNullException.ThrowIfNull(min);
            ArgumentNullException.ThrowIfNull(max);
            ArgumentNullException.ThrowIfNull(value);
            ArgumentNullException.ThrowIfNull(exception);

            isBetween = Is.Between(value, min, max, comparison);

            if (isBetween)
            {
                Throw.That(exception);
            }
        }
        finally
        {
            callback?.Invoke(isBetween ? GuardOutcome.Failure : GuardOutcome.Success);
        }
    }
}