using Microsoft.Extensions.Options;
using MVC.Extensions;
using NSE.WebApp.MVC.Models;

namespace MVC.Services;

public class CatalogoService : Service, ICatalogoService
{
    private readonly HttpClient _httpClient;

    public CatalogoService(HttpClient httpClient,
                           IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.CatalogoUrl
            ?? throw new ArgumentNullException(nameof(settings.Value.CatalogoUrl)));
    }

    public async Task<ProdutoViewModel> ObterPorId(Guid id)
    {
        var response = await _httpClient.GetAsync($"/catalogo/produtos/{id}");

        HttpResponseHasErrors(response);

        return await DeserializarObjetoResponse<ProdutoViewModel>(response);
    }

    public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
    {
        var response = await _httpClient.GetAsync("/catalogo/produtos");

        HttpResponseHasErrors(response);

        return await DeserializarObjetoResponse<IEnumerable<ProdutoViewModel>>(response);
    }
}
