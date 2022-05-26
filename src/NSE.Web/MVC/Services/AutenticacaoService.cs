using System.Text;
using System.Text.Json;
using NSE.WebApp.MVC.Models;

namespace MVC.Services;

public class AutenticacaoService : IAutenticacaoService
{
    private readonly HttpClient _httpClient;

    public AutenticacaoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuario)
    {
        var loginContent = new StringContent(
            content: JsonSerializer.Serialize(usuario),
            encoding: Encoding.UTF8,
            mediaType: "application/json");

        var response = await _httpClient.PostAsync(
            requestUri: "https://localhost:7044/api/identidade/autenticar",
            content: loginContent);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync(), options);

        return result;
    }

    public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuario)
    {
        var registroContent = new StringContent(
            content: JsonSerializer.Serialize(usuario),
            encoding: Encoding.UTF8,
            mediaType: "application/json");

        var response = await _httpClient.PostAsync(
            requestUri: "https://localhost:7044/api/identidade/nova-conta",
            content: registroContent);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync(), options);

        return result;
    }
}
