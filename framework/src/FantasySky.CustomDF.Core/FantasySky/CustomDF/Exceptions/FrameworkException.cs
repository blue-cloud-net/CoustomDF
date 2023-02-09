namespace FantasySky.CustomDF.Exceptions;

/// <summary>
/// Base exception type for those are thrown by Abp system for framework specific exceptions.
/// </summary>
public class FrameworkException : Exception
{
    public FrameworkException()
    {
    }

    public FrameworkException(string message)
        : base(message)
    {
    }

    public FrameworkException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
