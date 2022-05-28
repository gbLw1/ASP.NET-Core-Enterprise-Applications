using NSE.WebApp.MVC.Models;

namespace MVC.Services;

public class AutenticacaoService : Service, IAutenticacaoService
{
    private readonly HttpClient _httpClient;

    public AutenticacaoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuario)
    {
        var loginContent = ObterConteudo(usuario);

        var response = await _httpClient.PostAsync(
            requestUri: "https://localhost:7044/api/identidade/autenticar",
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
            requestUri: "https://localhost:7044/api/identidade/nova-conta",
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
