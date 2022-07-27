using OsduLib.Models;

namespace OsduLib.Exceptions;

public class OsduHttpException : Exception
{
    public ErrorResponse ErrorResponse;

    public OsduHttpException(ErrorResponse error, HttpRequestException innerException) : base(
        $"OSDU Error - {error.Message}",
        innerException)
    {
        ErrorResponse = error;
    }
}