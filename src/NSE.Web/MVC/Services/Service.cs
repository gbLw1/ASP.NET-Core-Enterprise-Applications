using Core.Comunication;
using MVC.Extensions;
using System.Text;
using System.Text.Json;

namespace MVC.Services;

public abstract class Service
{
    protected StringContent ObterConteudo(object dado)
    {
        return new StringContent(
            content: JsonSerializer.Serialize(dado),
            encoding: Encoding.UTF8,
            mediaType: "application/json");
    }

    protected async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage response)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        return JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), options)
        ?? throw new ArgumentNullException(nameof(response));
    }

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

    protected ResponseResult RetornoOk()
    {
        return new ResponseResult();
    }
}
