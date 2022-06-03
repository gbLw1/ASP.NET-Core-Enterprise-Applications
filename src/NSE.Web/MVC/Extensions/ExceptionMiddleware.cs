using System.Net;
using Polly.CircuitBreaker;
using Refit;

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
        catch (ValidationApiException ex) // Refit (403)
        {
            HandleResponseExceptionAsync(httpContext, ex.StatusCode);
        }
        catch (ApiException ex) // Refit (401)
        {
            HandleResponseExceptionAsync(httpContext, ex.StatusCode);
        }
        catch (BrokenCircuitException) // CircuitException
        {
            HandleResponseExceptionAsync(httpContext);
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

#region Refit

    /* ------------- Refactor: Método refatorado para o uso do Refit ------------ */
    // ! O refit tem suas tratativas de exception próprias.
    // ! Portanto é necessário adicionar outros "catchs" para tratativas dos erros.
    // ! ex: ValidationApiException: 403
    // ! ex: ApiException: 401

    private static void HandleResponseExceptionAsync(HttpContext httpContext, HttpStatusCode statusCode)
    {
        if (statusCode == HttpStatusCode.Unauthorized)
        {
            httpContext.Response.Redirect($"/login?ReturnUrl={httpContext.Request.Path}");
            return;
        }

        httpContext.Response.StatusCode = (int)statusCode;
    }

#endregion

#region CircuitBraker

    /* --------- Refactor: Método refatorado para o uso do CircuitBraker -------- */
    // ! É necessário adicionar o catch do CircuitBraker
    // ! ex: BrokenCircuitException

    private static void HandleResponseExceptionAsync(HttpContext httpContext)
    {
        httpContext.Response.Redirect("/sistema-indisponivel");
    }

#endregion
}
