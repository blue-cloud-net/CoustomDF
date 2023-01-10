namespace FantasySky.CustomDF.Exceptions;

public class InitializationException : FrameworkException
{
    public InitializationException()
    {

    }

    public InitializationException(string message)
        : base(message)
    {

    }

    public InitializationException(string message, Exception innerException)
        : base(message, innerException)
    {

    }
}
