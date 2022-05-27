using System.Net;

namespace MVC.Extensions;

public class CustomHttpResponseException : Exception
{
    public HttpStatusCode StatusCode;

    public CustomHttpResponseException() { }

    public CustomHttpResponseException(string message, Exception innerException)
        : base(message, innerException) { }

    public CustomHttpResponseException(HttpStatusCode statusCode)
    {
        StatusCode = statusCode;
    }
}
