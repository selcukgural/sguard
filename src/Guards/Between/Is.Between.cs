using System.Diagnostics.CodeAnalysis;

namespace SGuard;

/// <summary>
/// Provides a set of static methods to evaluate and validate conditions
/// related to comparisons of values, including checks for ranges and
/// relative orderings.
/// </summary>
public sealed partial class Is
{
    /// <summary>
    /// Determines whether the specified value lies within the inclusive range defined
    /// by the provided minimum and maximum values.
    /// </summary>
    /// <typeparam name="TValue">The type of the value being checked.</typeparam>
    /// <typeparam name="TMin">The type of the minimum boundary value.</typeparam>
    /// <typeparam name="TMax">The type of the maximum boundary value.</typeparam>
    /// <param name="value">The value to be checked for being within the range.</param>
    /// <param name="min">The minimum value of the range.</param>
    /// <param name="max">The maximum value of the range.</param>
    /// <param name="callback">
    /// An optional callback that will be invoked with the outcome of the evaluation,
    /// indicating success or failure of the check.
    /// </param>
    /// <returns>
    /// Returns <c>true</c> if the value is greater than or equal to the minimum
    /// and less than or equal to the maximum; otherwise, <c>false</c>.
    /// </returns>
    public static bool Between<TValue, TMin, TMax>([NotNull] TValue value, [NotNull]TMin min, [NotNull]TMax max, SGuardCallback? callback = null)
        where TValue : IComparable<TMin>, IComparable<TMax>
    {
        var isBetween = false;

        try
        {
            ArgumentNullException.ThrowIfNull(min);
            ArgumentNullException.ThrowIfNull(max);
            ArgumentNullException.ThrowIfNull(value);
            
            isBetween = value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
            return isBetween;
        }
        finally
        {
            callback?.Invoke(isBetween ? GuardOutcome.Success : GuardOutcome.Failure);
        }
    }
}