using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using SGuard.Exceptions;
using SGuard.Option;

namespace SGuard;

public partial class Is
{
    /// <summary>
    /// Throws an <see cref="AllNullException"/> if all elements in the given enumerable are null.
    /// </summary>
    /// <param name="param">The enumerable to check.</param>
    /// <param name="condition">
    /// A boolean value indicating whether to throw the exception or not. 
    /// The exception will be thrown if this value is true.
    /// </param>
    public static void AllNullThrow(IEnumerable<object?>? param, [DoesNotReturnIf(true)] bool condition = true)
    {
        AllNullThrow(new AllNullException("All element are null"), true, param);
    }

    /// <summary>
    /// Throws a given exception if all elements in the given enumerable are null.
    /// </summary>
    /// <typeparam name="TException">The type of the exception to throw.</typeparam>
    /// <param name="exception">The exception to throw.</param>
    /// <param name="condition">
    /// A boolean value indicating whether to throw the exception or not. 
    /// The exception will be thrown if this value is true.
    /// </param>
    /// <param name="param">The enumerable to check.</param>
    public static void AllNullThrow<TException>([NotNull] TException exception, [DoesNotReturnIf(true)] bool condition = true, params object?[]? param) where TException : Exception
    {
        if (param == null)
        {
            throw new ArgumentNullException(nameof(param));
        }

        if (param.Any(e => e != null))
        {
            return;
        }

        throw exception;
    }

    /// <summary>
    /// Throws an <see cref="IsNullOrEmptyException"/> if the given value is null or empty.
    /// </summary>
    /// <typeparam name="T">The type of the value to check.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="option">
    /// A callback action that can be used to customize the behavior of this method.
    /// This can be used to specify a custom callback to be invoked instead of throwing an exception.
    /// </param>
    public static void NullOrEmptyThrow<T>(T? value, Action<CallbackOption>? option = null)
    {
        var opt = new CallbackOption();
        option?.Invoke(opt);

        var isNull = NullOrEmpty(value, option);

        if (!isNull)
        {
            return;
        }

        if (!opt.IsNullThrowFailure && opt.HasCallback())
        {
            opt.InvokeCallback();

            return;
        }

        Throw.IsNullOrEmptyException();
    }

    /// <summary>
    /// Throws an exception if the given value is null or empty.
    /// </summary>
    /// <typeparam name="T">The type of the value being checked.</typeparam>
    /// <typeparam name="TException">The type of exception to throw if the value is null or empty.</typeparam>
    /// <param name="value">The value to check for null or empty.</param>
    /// <param name="exception">The exception to throw if the value is null or empty.</param>
    /// <param name="condition">A boolean value indicating whether to throw the exception. If this value is false, the exception will not be thrown. Default value is true.</param>
    public static void NullOrEmptyThrow<T, TException>(T? value, [NotNull] TException exception, [DoesNotReturnIf(true)] bool condition = true) where TException : Exception
    {
        if (NullOrEmpty(value, isOption: null))
        {
            throw exception;
        }
    }

    /// <summary>
    /// Throws an <see cref="IsNullOrEmptyException"/> if the given value or the value accessed through the given expression is null or empty.
    /// </summary>
    /// <typeparam name="TValue">The type of the object containing the value to check.</typeparam>
    /// <param name="obj">The object containing the value to check.</param>
    /// <param name="value">An expression that accesses the value to check within the object.</param>
    /// <param name="option">
    /// A callback action that can be used to customize the behavior of this method.
    /// This can be used to specify a custom callback to be invoked instead of throwing an exception.
    /// </param>
    public static void NullOrEmptyThrow<TValue>(TValue? obj, Expression<Func<TValue, object>>? value, Action<CallbackOption>? option = null)
    {
        var isNull = NullOrEmpty(obj, value, option);

        if (!isNull)
        {
            return;
        }

        var opt = new CallbackOption();
        option?.Invoke(opt);

        if (!opt.IsNullThrowFailure && opt.HasCallback())
        {
            opt.InvokeCallback();

            return;
        }

        Throw.IsNullOrEmptyException();
    }
    /// <summary>
    /// Throws a given exception if the given value or the value accessed through the given expression is null or empty.
    /// </summary>
    /// <typeparam name="TValue">The type of the object containing the value to check.</typeparam>
    /// <typeparam name="TException">The type of the exception to throw.</typeparam>
    /// <param name="obj">The object containing the value to check.</param>
    /// <param name="value">An expression that accesses the value to check within the object.</param>
    /// <param name="exception">The exception to throw.</param>
    /// <param name="condition">
    /// A boolean value indicating whether to throw the exception or not. 
    /// The exception will be thrown if this value is true.
    /// </param>
    public static void NullOrEmptyThrow<TValue, TException>(TValue? obj, Expression<Func<TValue, object>>? value, [NotNull] TException exception, [DoesNotReturnIf(true)] bool condition = true) where TException : Exception
    {
        if (NullOrEmpty(obj, value, option: null))
        {
            throw exception;
        }
    }
    /// <summary>
    /// Throws an exception if the specified value is not between the specified minimum and maximum values.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to compare.</typeparam>
    /// <typeparam name="TMin">The type of the minimum value.</typeparam>
    /// <typeparam name="TMax">The type of the maximum value.</typeparam>
    /// <param name="value">The value to compare.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="option">An optional callback to execute before throwing the exception. 
    /// If the callback returns <see cref="CallbackOption"/>, the exception will not be thrown.</param>
    /// <exception cref="IsBetweenException">Thrown if the value is not between the minimum and maximum values.</exception>
    public static void BetweenThrow<TValue, TMin, TMax>([NotNull] TValue value, [NotNull] TMin min,
        [NotNull] TMax max,
        Action<CallbackOption>? option = null)
        where TMax : TValue, new()
        where TMin : TValue, new()
        where TValue : IComparable, new()
    {
        if (Between(value, min, max, option))
        {
            Throw.IsBetweenException(value,min,max);
        }
    }
    /// <summary>
    /// Throws the provided exception if the value is between the specified minimum and maximum values.
    /// </summary>
    /// <typeparam name="TValue">The type of the value to be checked.</typeparam>
    /// <typeparam name="TMin">The type of the minimum value. Must be a subtype of TValue.</typeparam>
    /// <typeparam name="TMax">The type of the maximum value. Must be a subtype of TValue.</typeparam>
    /// <typeparam name="TException">The type of the exception to be thrown. Must be a subtype of Exception.</typeparam>
    /// <param name="value">The value to be checked.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="exception">The exception to be thrown.</param>
    /// <param name="condition">An optional boolean condition. If provided and set to true, the exception will be thrown. If not provided or set to false, the exception will not be thrown.</param>
    public static void BetweenThrow<TValue, TMin, TMax,TException>([NotNull] TValue value, [NotNull] TMin min,
        [NotNull] TMax max,
       [NotNull]TException exception,
        [DoesNotReturnIf(true)] bool condition = true)
        where TMax : TValue, new()
        where TMin : TValue, new()
        where TValue : IComparable, new()
    where TException : Exception
    {
        if (Between(value, min, max))
        {
            throw exception;
        }
    }
    /// <summary>
    /// Throws an exception if the left value is greater than the right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value. Must be a subtype of TLeft.</typeparam>
    /// <param name="lValue">The left value.</param>
    /// <param name="rValue">The right value.</param>
    /// <param name="option">An optional callback for customizing the exception that is thrown. If not provided, a default exception will be thrown.</param>
    public static void GreaterThanThrow<TLeft, TRight>([NotNull] TLeft lValue, [NotNull] TRight rValue,
        Action<CallbackOption>? option = null) where TLeft : IComparable
        where TRight : TLeft
    {
        if (GreaterThan(lValue, rValue, option))
        {
            Throw.IsGreaterThanException(lValue, rValue);
        }
    }
    /// <summary>
    /// Throws the provided exception if the left value is greater than the right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value. Must be a subtype of TLeft.</typeparam>
    /// <typeparam name="TException">The type of the exception to be thrown. Must be a subtype of Exception.</typeparam>
    /// <param name="lValue">The left value.</param>
    /// <param name="rValue">The right value.</param>
    /// <param name="exception">The exception to be thrown.</param>
    /// <param name="condition">An optional boolean condition. If provided and set to true, the exception will be thrown. If not provided or set to false, the exception will not be thrown.</param>
    public static void GreaterThanThrow<TLeft, TRight, TException>([NotNull] TLeft lValue, [NotNull] TRight rValue,
        [NotNull] TException exception, [DoesNotReturnIf(true)] bool condition = true) 
        where TLeft : IComparable
        where TRight : TLeft
        where TException : Exception
    {
        if (GreaterThan(lValue, rValue))
        {
            throw exception;
        }
    }
    /// <summary>
    /// Throws an exception if the left value is not greater than or equal to the right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value.</typeparam>
    /// <typeparam name="TRight">The type of the right value. Must be a subtype of <typeparamref name="TLeft"/>.</typeparam>
    /// <param name="lValue">The left value to compare.</param>
    /// <param name="rValue">The right value to compare.</param>
    /// <param name="option">An optional action to customize the exception message or perform additional actions before throwing the exception.</param>
    /// <exception cref="IsGreaterOrEqualThanException">Thrown if the left value is not greater than or equal to the right value.</exception>
    public static void GreaterOrEqualThanThrow<TLeft, TRight>([NotNull] TLeft lValue, [NotNull] TRight rValue,
        Action<CallbackOption>? option = null)
        where TLeft : IComparable
        where TRight : TLeft
    {
        if (GreaterOrEqualThan(lValue, rValue, option))
        {
            Throw.IsGreaterOrEqualThanException(lValue,rValue);
        }
    }
    /// <summary>
    /// Throws an exception if the left value is greater than or equal to the right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value. Must implement IComparable.</typeparam>
    /// <typeparam name="TRight">The type of the right value. Must be a subtype of TLeft.</typeparam>
    /// <typeparam name="TException">The type of exception to throw. Must be a subclass of Exception.</typeparam>
    /// <param name="lValue">The left value to compare.</param>
    /// <param name="rValue">The right value to compare.</param>
    /// <param name="exception">The exception to throw if the left value is greater than or equal to the right value.</param>
    /// <param name="condition">An optional condition to check before throwing the exception. If this evaluates to true, the exception will be thrown. Default is true.</param>
    /// <exception cref="Exception">Thrown if the left value is greater than or equal to the right value and the condition evaluates to true.</exception>
    public static void GreaterOrEqualThanThrow<TLeft, TRight,TException>([NotNull] TLeft lValue, [NotNull] TRight rValue,
        [NotNull]TException exception, [DoesNotReturnIf(true)] bool condition = true)
        where TLeft : IComparable
        where TRight : TLeft
    where TException : Exception
    {
        if (GreaterOrEqualThan(lValue, rValue))
        {
            throw exception;
        }
    }
    /// <summary>
    /// Throws an exception if the left value is less than the right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value. Must implement IComparable.</typeparam>
    /// <typeparam name="TRight">The type of the right value. Must be a subtype of TLeft.</typeparam>
    /// <param name="lValue">The left value to compare.</param>
    /// <param name="rValue">The right value to compare.</param>
    /// <param name="option">An optional action to perform before throwing the exception. Default is null.</param>
    /// <exception cref="IsLessThanException">Thrown if the left value is less than the right value.</exception>
    public static void LessThanThrow<TLeft, TRight>([NotNull] TLeft lValue, [NotNull] TRight rValue,
        Action<CallbackOption>? option = null)
        where TLeft : IComparable
        where TRight : TLeft
    {
        if (LessThan(lValue, rValue, option))
        {
            Throw.IsLessThanException(lValue,rValue);
        }
    }
    /// <summary>
    /// Throws an exception if the left value is less than the right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value. Must implement IComparable.</typeparam>
    /// <typeparam name="TRight">The type of the right value. Must be a subtype of TLeft.</typeparam>
    /// <typeparam name="TException">The type of exception to throw. Must be a subclass of Exception.</typeparam>
    /// <param name="lValue">The left value to compare.</param>
    /// <param name="rValue">The right value to compare.</param>
    /// <param name="exception">The exception to throw if the left value is less than the right value.</param>
    /// <param name="condition">An optional condition to check before throwing the exception. If this evaluates to true, the exception will be thrown. Default is true.</param>
    /// <exception cref="Exception">Thrown if the left value is less than the right value and the condition evaluates to true.</exception>
    public static void LessThanThrow<TLeft, TRight, TException>([NotNull] TLeft lValue, [NotNull] TRight rValue,
        [NotNull]TException exception, [DoesNotReturnIf(true)]bool condition = true)
        where TLeft : IComparable
        where TRight : TLeft
    where TException : Exception
    {
        if (LessThan(lValue, rValue))
        {
            throw exception;
        }
    }
    /// <summary>
    /// Throws an exception if the left value is less than or equal to the right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value. Must implement IComparable.</typeparam>
    /// <typeparam name="TRight">The type of the right value. Must be a subtype of TLeft.</typeparam>
    /// <param name="lValue">The left value to compare.</param>
    /// <param name="rValue">The right value to compare.</param>
    /// <param name="option">An optional action to perform before throwing the exception. Default is null.</param>
    /// <exception cref="IsLessThanOrEqualException">Thrown if the left value is less than or equal to the right value.</exception>
    public static void LessOrEqualThanThrow<TLeft, TRight>([NotNull] TLeft lValue, [NotNull] TRight rValue,
        Action<CallbackOption>? option = null)
        where TLeft : IComparable
        where TRight : TLeft
    {
        if (LessOrEqualThan(lValue, rValue, option))
        {
            Throw.IsLessOrEqualThanException(lValue,rValue);
        }
    }
    /// <summary>
    /// Throws an exception if the left value is less than or equal to the right value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the left value. Must implement IComparable.</typeparam>
    /// <typeparam name="TRight">The type of the right value. Must be a subtype of TLeft.</typeparam>
    /// <typeparam name="TException">The type of exception to throw. Must be a subclass of Exception.</typeparam>
    /// <param name="lValue">The left value to compare.</param>
    /// <param name="rValue">The right value to compare.</param>
    /// <param name="exception">The exception to throw if the left value is less than or equal to the right value.</param>
    /// <param name="condition">An optional condition to check before throwing the exception. If this evaluates to true, the exception will be thrown. Default is true.</param>
    /// <exception cref="Exception">Thrown if the left value is less than or equal to the right value and the condition evaluates to true.</exception>
    public static void LessOrEqualThanThrow<TLeft, TRight, TException>([NotNull] TLeft lValue, [NotNull] TRight rValue,
        [NotNull]TException exception, [DoesNotReturnIf(true)] bool condition = true)
        where TLeft : IComparable
        where TRight : TLeft
    where TException : Exception
    {
        if (LessOrEqualThan(lValue, rValue))
        {
            throw exception;
        }
    }
}