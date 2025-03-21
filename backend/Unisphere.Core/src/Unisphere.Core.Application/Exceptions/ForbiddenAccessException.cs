namespace Unisphere.Core.Application.Exceptions;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException()
    : base("This action is not allowed")
    {
    }

    public ForbiddenAccessException(string message)
    : base(message)
    {
    }

    public ForbiddenAccessException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
