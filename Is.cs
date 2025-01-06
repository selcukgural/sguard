using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using SGuard.Visitor;

namespace SGuard;

public sealed partial class Is
{
    private static readonly NullVisitor NullVisitor = new();
    private static Action? _globalCallback;
    private static Action<string?>? _globalCallback2;

    /// <summary>
    /// Sets a global callback action to be invoked when a nullity or emptiness check is performed.
    /// </summary>
    /// <param name="globalCallback">The action to be invoked.</param>
    public static void SetCallback(Action globalCallback)
    {
        _globalCallback = globalCallback;
    }
    /// <summary>
    /// Sets a global callback action to be invoked when a nullity or emptiness check is performed.
    /// </summary>
    /// <param name="globalCallback">The action to be invoked.</param>
    public static void SetCallback(Action<string?>? globalCallback)
    {
        _globalCallback2 = globalCallback;
    }
    /// <summary>
    /// Determines if all objects in the given array are null.
    /// </summary>
    /// <param name="param">The array of objects to check.</param>
    /// <returns>True if all objects are null, false otherwise.</returns>
    public static bool AllNull(params object?[]? param)
    {
        return param == null || param.All(e => e == default);
    }
    /// <summary>
    /// Determines if the given object is null or empty.
    /// </summary>
    /// <typeparam name="T">The type of the object. Must be a reference type.</typeparam>
    /// <param name="value">The object to check.</param>
    /// <param name="isOption">An optional action to perform before checking for nullity or emptiness.</param>
    /// <returns>True if the object is null or empty, false otherwise.</returns>
    private static bool NullOrEmpty<T>(T? value, CallbackOption? isOption = null)
    {
        if (value == null)
        {
            InvokeCallbacks(isOption);
            return true;
        }

        switch (value)
        {
            case string str:
                if (!string.IsNullOrEmpty(str))
                {
                    return false;
                }
                break;

            case char chr:
                if (chr != default)
                {
                    return false;
                }
                break;

            case byte or sbyte or ushort or int or uint or long or ulong or float or double or decimal:
                if (Convert.ToDecimal(value) != 0)
                {
                    return false;
                }
                break;

            case bool boolVal:
                if (boolVal)
                {
                    return false;
                }
                break;

            case Guid guid:
                if (guid != Guid.Empty)
                {
                    return false;
                }
                break;

            case IDictionary dictionary:
                if (dictionary.Count != 0)
                {
                    return false;
                }
                break;

            case DateTime datetime:
                if (datetime.Ticks != 0)
                {
                    return false;
                }
                break;

            case TimeSpan timespan:
                if (timespan.Ticks != TimeSpan.MinValue.Ticks)
                {
                    return false;
                }
                break;

            case DateOnly dateOnly:
                if (dateOnly != default)
                {
                    return false;
                }
                break;

            case TimeOnly timeOnly:
                if (timeOnly != default)
                {
                    return false;
                }
                break;

            case DateTimeOffset dateTimeOffset:
                if (dateTimeOffset != default)
                {
                    return false;
                }
                break;

            case IEnumerable<T> enumerable:
                if (enumerable.Cast<object?>().Any(item => item != null))
                {
                    return false;
                }
                break;

            case Array array:
                if (array.Cast<object?>().Any(item => item != null))
                {
                    return false;
                }
                break;

            case IList list:
                if (list.Cast<object?>().Any(item => item != null))
                {
                    return false;
                }
                break;

            case Type:
                return false;

            case IEnumerable list:
                if (list.Cast<object?>().Any(item => item != null))
                {
                    return false;
                }
                break;

            default:
                var valueType = value.GetType();
                var properties = valueType.GetProperties();

                if (valueType is { IsValueType: true, IsEnum: false } || valueType.IsClass)
                {
                    if (properties.Any(e => e.CanRead && !string.IsNullOrEmpty(e.GetValue(value)?.ToString())))
                    {
                        return false;
                    }
                }
                else
                {
                    throw new InvalidOperationException();
                }
                break;
        }

        InvokeCallbacks(isOption);
        return true;
    }
    /// <summary>
    /// Determines if the given object is null or empty.
    /// </summary>
    /// <typeparam name="T">The type of the object. Must be a reference type.</typeparam>
    /// <param name="value">The object to check.</param>
    /// <param name="option">An optional action to perform before checking for nullity or emptiness.</param>
    /// <returns>True if the object is null or empty, false otherwise.</returns>
    public static bool NullOrEmpty<T>(T? value, Action<CallbackOption>? option = null)
    {
        var isOption = new CallbackOption();
        option?.Invoke(isOption);
        
        return NullOrEmpty(value, isOption);
    }
    /// <summary>
    /// Determines if the given object is null or empty.
    /// </summary>
    /// <typeparam name="T">The type of the object. Must be a reference type.</typeparam>
    /// <param name="obj">The object to check.</param>
    /// <param name="value">The property of the object to check for nullity or emptiness.</param>
    /// <param name="option">An optional action to perform before checking for nullity or emptiness.</param>
    /// <returns>True if the object is null or empty, false otherwise.</returns>
    private static bool NullOrEmpty<T>(T? obj, Expression<Func<T, object>>? value, CallbackOption? option = null)
    {
        if (obj == null)
        {
            InvokeCallbacks(option);
            return true;
        }

        var expression = NullVisitor.Visit(value) as Expression<Func<T, object>>;
        return expression?.Compile()(obj) == null;
    }
    /// <summary>
    /// Determines if the given object is null or empty.
    /// </summary>
    /// <typeparam name="T">The type of the object. Must be a reference type.</typeparam>
    /// <param name="obj">The object to check.</param>
    /// <param name="value">The property of the object to check for nullity or emptiness.</param>
    /// <param name="options">An optional action to perform before checking for nullity or emptiness.</param>
    /// <returns>True if the object is null or empty, false otherwise.</returns>
    public static bool NullOrEmpty<T>(T? obj, Expression<Func<T, object>>? value, Action<CallbackOption>? options = null)
    {
        var option = new CallbackOption();
        options?.Invoke(option);

        return NullOrEmpty(obj, value, option);
    }
    /// <summary>
    /// Determines if the given value is between the specified minimum and maximum values.
    /// </summary>
    /// <typeparam name="TValue">The type of the value. Must implement IComparable.</typeparam>
    /// <typeparam name="TMin">The type of the minimum value. Must be a subtype of TValue.</typeparam>
    /// <typeparam name="TMax">The type of the maximum value. Must be a subtype of TValue.</typeparam>
    /// <param name="value">The value to check.</param>
    /// <param name="min">The minimum value.</param>
    /// <param name="max">The maximum value.</param>
    /// <param name="option">An optional action to perform before checking if the value is between the minimum and maximum values.</param>
    /// <returns>True if the value is between the minimum and maximum values, false otherwise.</returns>
    /// <exception cref="ArgumentException">Thrown if the types of TValue, TMin, and TMax are not the same.</exception>
    public static bool Between<TValue, TMin, TMax>([NotNull] TValue value, [NotNull] TMin min, [NotNull] TMax max, Action<CallbackOption>? option = null)
        where TMax : TValue, new()
        where TMin : TValue, new()
        where TValue : IComparable, new()
    {
        var options = new CallbackOption();
        option?.Invoke(options);

        if (typeof(TValue) != typeof(TMin) || typeof(TValue) != typeof(TMax))
        {
            throw new ArgumentException($"{nameof(TValue)}, {nameof(TMin)} and {nameof(TMax)} must be some type.");
        }

        if (Comparer<TValue>.Default.Compare(value, min) >= 0 &&
            Comparer<TValue>.Default.Compare(value, max) <= 0)
        {
            return true;
        }

        InvokeCallbacks(options);
        return false;
    }
    /// <summary>
    /// Determines if the first value is greater than the second value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the first value. Must implement IComparable.</typeparam>
    /// <typeparam name="TRight">The type of the second value. Must be a subtype of TLeft.</typeparam>
    /// <param name="lValue">The first value.</param>
    /// <param name="rValue">The second value.</param>
    /// <param name="option">An optional action to perform before checking if the first value is greater than the second value.</param>
    /// <returns>True if the first value is greater than the second value, false otherwise.</returns>
    public static bool GreaterThan<TLeft, TRight>([NotNull] TLeft lValue, [NotNull] TRight rValue, Action<CallbackOption>? option = null)
        where TLeft : IComparable
        where TRight : TLeft
    {
        var options = new CallbackOption();
        option?.Invoke(options);

        if (Comparer<TLeft>.Default.Compare(lValue, rValue) > 0)
        {
            return true;
        }

        InvokeCallbacks(options);
        return false;
    }
    /// <summary>
    /// Determines if the first value is greater than or equal to the second value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the first value. Must implement IComparable.</typeparam>
    /// <typeparam name="TRight">The type of the second value. Must be a subtype of TLeft.</typeparam>
    /// <param name="lValue">The first value.</param>
    /// <param name="rValue">The second value.</param>
    /// <param name="option">An optional action to perform before checking if the first value is greater than or equal to the second value.</param>
    /// <returns>True if the first value is greater than or equal to the second value, false otherwise.</returns>
    public static bool GreaterOrEqualThan<TLeft, TRight>([NotNull] TLeft lValue, [NotNull] TRight rValue, Action<CallbackOption>? option = null)
        where TLeft : IComparable
        where TRight : TLeft
    {
        var options = new CallbackOption();
        option?.Invoke(options);

        if (Comparer<TLeft>.Default.Compare(lValue, rValue) >= 0)
        {
            return true;
        }

        InvokeCallbacks(options);
        return false;
    }
    /// <summary>
    /// Determines if the first value is less than the second value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the first value. Must implement IComparable.</typeparam>
    /// <typeparam name="TRight">The type of the second value. Must be a subtype of TLeft.</typeparam>
    /// <param name="lValue">The first value.</param>
    /// <param name="rValue">The second value.</param>
    /// <param name="option">An optional action to perform before checking if the first value is less than the second value.</param>
    /// <returns>True if the first value is less than the second value, false otherwise.</returns>
    public static bool LessThan<TLeft, TRight>([NotNull] TLeft lValue, [NotNull] TRight rValue, Action<CallbackOption>? option = null)
        where TLeft : IComparable
        where TRight : TLeft
    {
        var options = new CallbackOption();
        option?.Invoke(options);

        if (Comparer<TLeft>.Default.Compare(lValue, rValue) < 0)
        {
            return true;
        }

        InvokeCallbacks(options);
        return false;
    }
    /// <summary>
    /// Determines if the first value is less than or equal to the second value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the first value. Must implement IComparable.</typeparam>
    /// <typeparam name="TRight">The type of the second value. Must be a subtype of TLeft.</typeparam>
    /// <param name="lValue">The first value.</param>
    /// <param name="rValue">The second value.</param>
    /// <param name="option">An optional action to perform before checking if the first value is less than or equal to the second value.</param>
    /// <returns>True if the first value is less than or equal to the second value, false otherwise.</returns>
    public static bool LessOrEqualThan<TLeft, TRight>([NotNull] TLeft lValue, [NotNull] TRight rValue, Action<CallbackOption>? option = null)
        where TLeft : IComparable
        where TRight : TLeft
    {
        var options = new CallbackOption();
        option?.Invoke(options);

        if (Comparer<TLeft>.Default.Compare(lValue, rValue) <= 0)
        {
            return true;
        }

        InvokeCallbacks(options);
        return false;
    }
    /// <summary>
    /// Invokes the callbacks specified in the isOption parameter or the global callbacks specified using the SetCallback method.
    /// </summary>
    /// <param name="isOption">An optional callback option object.</param>
    private static void InvokeCallbacks(CallbackOption? isOption)
    {
        if (isOption is {InvokeCallbackWhenNullOrEmpty: true})
        {
            isOption.InvokeCallback();
            return;
        }
        
        _globalCallback?.Invoke();
        _globalCallback2?.Invoke(null);
    }
}