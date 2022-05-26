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

    public async Task<string> Login(UsuarioLogin usuario)
    {
        var loginContent = new StringContent(
            content: JsonSerializer.Serialize(usuario),
            encoding: Encoding.UTF8,
            mediaType: "application/json");

        var response = await _httpClient.PostAsync(
            requestUri: "https://localhost:7044/api/identidade/autenticar",
            content: loginContent);

        var result = JsonSerializer.Deserialize<string>(await response.Content.ReadAsStringAsync());

        if (string.IsNullOrWhiteSpace(result))
        {
            throw new ArgumentException();
        }

        return result;
    }

    public async Task<string> Registro(UsuarioRegistro usuario)
    {
        var registroContent = new StringContent(
            content: JsonSerializer.Serialize(usuario),
            encoding: Encoding.UTF8,
            mediaType: "application/json");

        var response = await _httpClient.PostAsync(
            requestUri: "https://localhost:7044/api/identidade/nova-conta",
            content: registroContent);

        var result = JsonSerializer.Deserialize<string>(await response.Content.ReadAsStringAsync());

        if (string.IsNullOrWhiteSpace(result))
        {
            throw new ArgumentException();
        }

        return result;
    }
}
