using Core.Comunication;
using Microsoft.Extensions.Options;
using MVC.Extensions;
using NSE.WebApp.MVC.Models;

namespace MVC.Services;

public interface IAutenticacaoService
{
    Task<UsuarioRespostaLogin> Login(UsuarioLogin usuario);
    Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuario);
}

public class AutenticacaoService : Service, IAutenticacaoService
{
    private readonly HttpClient _httpClient;

    public AutenticacaoService(HttpClient httpClient,
                               IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.AutenticacaoUrl
            ?? throw new ArgumentNullException(nameof(settings.Value.AutenticacaoUrl)));
    }

    public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuario)
    {
        var loginContent = ObterConteudo(usuario);

        var response = await _httpClient.PostAsync(
            requestUri: "/api/identidade/autenticar",
            content: loginContent);

        if (HttpResponseHasErrors(response))
        {
            return new UsuarioRespostaLogin
            {
                ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
            };
        }

        return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
    }

    public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuario)
    {
        var registroContent = ObterConteudo(usuario);

        var response = await _httpClient.PostAsync(
            requestUri: "/api/identidade/nova-conta",
            content: registroContent);

        if (HttpResponseHasErrors(response))
        {
            return new UsuarioRespostaLogin
            {
                ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
            };
        }

        return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
    }
}
