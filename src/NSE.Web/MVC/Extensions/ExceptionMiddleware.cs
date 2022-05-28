using System.Net;

namespace MVC.Extensions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (CustomHttpResponseException ex)
        {
            HandleResponseExceptionAsync(httpContext, ex);
        }
    }

    private static void HandleResponseExceptionAsync(HttpContext httpContext, CustomHttpResponseException httpResponseException)
    {
        if (httpResponseException.StatusCode == HttpStatusCode.Unauthorized)
        {
            httpContext.Response.Redirect($"/login?ReturnUrl={httpContext.Request.Path}");
            return;
        }

        httpContext.Response.StatusCode = (int)httpResponseException.StatusCode;
    }
}
