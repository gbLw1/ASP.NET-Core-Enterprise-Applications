using System.Net;
using System.Text;
using System.Text.Json;

namespace NSE.Bff.Compras.Services;

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
        if (response.StatusCode == HttpStatusCode.BadRequest) return false;

        response.EnsureSuccessStatusCode();
        return false;
    }
}
