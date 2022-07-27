namespace OsduLib.Exceptions;

public class InvalidTokenException : Exception
{
    public InvalidTokenException(Type tokenResponseType, string reason) : base(
        $"{tokenResponseType.Name} is invalid - {reason}")
    {
        TokenResponseType = tokenResponseType;
    }

    public Type TokenResponseType { get; }
}