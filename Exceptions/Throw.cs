using System.Diagnostics.CodeAnalysis;

namespace SGuard.Exceptions;

/// <summary>
/// An internal static class that throws exceptions for various conditions.
/// </summary>
internal static class Throw
{
    /// <summary>
    /// Throws an <see cref="IsBetweenException"/> if the given value is not between the given min and max values.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to check.</typeparam>
    /// <typeparam name="TMin">The type of the minimum value.</typeparam>
    /// <typeparam name="TMax">The type of the maximum value.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    [DoesNotReturn]
    public static void IsBetweenException<TValue, TMin, TMax>(TValue value, TMin min, TMax max)
    {
        throw new IsBetweenException($"Value {value}, Min {min}, Max {max}");
    }
    /// <summary>
    /// Throws an <see cref="AllNullException"/> if all elements are null or empty.
    /// </summary>
    [DoesNotReturn]
    public static void AllNullException()
    {
        throw new AllNullException("All element are null or empty");
    }
    /// <summary>
    /// Throws an <see cref="IsNullOrEmptyException"/> if the given value is null or empty.
    /// </summary>
    [DoesNotReturn]
    public static void IsNullOrEmptyException()
    {
        throw new IsNullOrEmptyException("Value is null or empty");
    }
    /// <summary>
    /// Throws an <see cref="IsGreaterThanException"/> if the left value is greater than the right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="lValue">The left value.</param>
    /// <param name="rValue">The right value.</param>
    [DoesNotReturn]
    public static void IsGreaterThanException<TLeft, TRight>(TLeft lValue, TRight rValue)
    {
        throw new IsGreaterThanException($"{lValue} greater than {rValue}");
    }

    /// <summary>
    /// Throws an <see cref="IsGreaterOrEqualThanException"/> if the left value is greater or equal to the right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="lValue">The left value.</param>
    /// <param name="rValue">The right value.</param>
    [DoesNotReturn]
    public static void IsGreaterOrEqualThanException<TLeft, TRight>(TLeft lValue, TRight rValue)
    {
        throw new IsGreaterOrEqualThanException($"{lValue} greater or equal than {rValue}");
    }

    /// <summary>
    /// Throws an <see cref="IsLessThanException"/> if the left value is less than the right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="lValue">The left value.</param>
    /// <param name="rValue">The right value.</param>
    [DoesNotReturn]
    public static void IsLessThanException<TLeft, TRight>(TLeft lValue, TRight rValue)
    {
        throw new IsLessThanException($"{lValue} less than {rValue}");
    }

    /// <summary>
    /// Throws an <see cref="IsLessThanOrEqualException"/> if the left value is less or equal to the right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value.</typeparam>
    /// <param name="lValue">The left value.</param>
    /// <param name="rValue">The right value.</param>
    [DoesNotReturn]
    public static void IsLessOrEqualThanException<TLeft, TRight>(TLeft lValue, TRight rValue)
    {
        throw new IsLessThanOrEqualException($"{lValue} less or equal than {rValue}");
    }
}