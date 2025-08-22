namespace SGuard;

public sealed partial class Is
{
    /// <summary>
    /// Determines whether the left value is less than the right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value to compare.</typeparam>
    /// <typeparam name="TRight">The type of the right value to compare.</typeparam>
    /// <param name="lValue">The left value to compare. Must not be null.</param>
    /// <param name="rValue">The right value to compare. Must not be null.</param>
    /// <param name="callback">
    /// An optional callback to invoke with the result of the comparison.
    /// The callback is invoked with <c>true</c> if the left value is less than the right value; otherwise, <c>false</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the left value is less than the right value; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="lValue"/> or <paramref name="rValue"/> is null.
    /// </exception>
    public static bool LessThan<TLeft, TRight>(TLeft lValue, TRight rValue, SGuardCallback? callback = null) where TLeft : IComparable<TRight>
    {
        var isLessThan = false;

        try
        {
            ArgumentNullException.ThrowIfNull(lValue);
            ArgumentNullException.ThrowIfNull(rValue);

            isLessThan = lValue.CompareTo(rValue) < 0;

            return isLessThan;
        }
        finally
        {
            callback?.Invoke(isLessThan ? GuardOutcome.Success : GuardOutcome.Failure);
        }
    }


    /// <summary>
    /// Determines whether the left value is less than or equal to the right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value to compare.</typeparam>
    /// <typeparam name="TRight">The type of the right value to compare.</typeparam>
    /// <param name="lValue">The left value to compare. Must not be null.</param>
    /// <param name="rValue">The right value to compare. Must not be null.</param>
    /// <param name="callback">
    /// An optional callback to invoke with the result of the comparison.
    /// The callback is invoked with <c>true</c> if the left value is less than or equal to the right value; otherwise, <c>false</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the left value is less than or equal to the right value; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="lValue"/> or <paramref name="rValue"/> is null.
    /// </exception>
    public static bool LessThanOrEqual<TLeft, TRight>(TLeft lValue, TRight rValue, SGuardCallback? callback = null) where TLeft : IComparable<TRight>
    {
        var isLessThanOrEqual = false;

        try
        {
            ArgumentNullException.ThrowIfNull(lValue);
            ArgumentNullException.ThrowIfNull(rValue);

            isLessThanOrEqual = lValue.CompareTo(rValue) <= 0;

            return isLessThanOrEqual;
        }
        finally
        {
            callback?.Invoke(isLessThanOrEqual ? GuardOutcome.Success : GuardOutcome.Failure);
        }
    }
}