using MVC.Extensions;

namespace MVC.Services;

public abstract class Service
{
    protected bool HttpResponseHasErrors(HttpResponseMessage response)
    {
        switch ((int)response.StatusCode)
        {
            case 401: // Unauthorized
            case 403: // Forbidden
            case 404: // Not Found
            case 500: // Internal Server Error
                throw new CustomHttpResponseException(response.StatusCode);

            case 400:
                return true;
        }

        response.EnsureSuccessStatusCode();
        return false;
    }
}
