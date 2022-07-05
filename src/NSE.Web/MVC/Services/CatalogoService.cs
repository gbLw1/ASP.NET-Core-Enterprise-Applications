using Microsoft.Extensions.Options;
using MVC.Extensions;
using NSE.WebApp.MVC.Models;

namespace MVC.Services;

public interface ICatalogoService
{
    Task<IEnumerable<ProdutoViewModel>> ObterTodos();
    Task<ProdutoViewModel> ObterPorId(Guid id);
}

// Refit
//public interface ICatalogoServiceRefit
//{
//    [Get("/catalogo/produtos")]
//    Task<IEnumerable<ProdutoViewModel>> ObterTodos();

//    [Get("/catalogo/produtos/{id}")]
//    Task<ProdutoViewModel> ObterPorId(Guid id);
//}

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
