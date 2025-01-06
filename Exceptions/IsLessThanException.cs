using System.Runtime.Serialization;

namespace SGuard.Exceptions;

/// <summary>
/// The exception that is thrown when all the objects in an array are null.
/// </summary>
[Serializable]
public sealed class IsLessThanException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IsLessThanException"/> class.
    /// </summary>
    public IsLessThanException()
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="IsLessThanException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public IsLessThanException(string message) : base(message)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="IsLessThanException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public IsLessThanException(string message, Exception inner) : base(message, inner)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="IsLessThanException"/> class with serialized data.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
    internal IsLessThanException(
        SerializationInfo info,
        StreamingContext context) : base(info, context)
    {
    }
}