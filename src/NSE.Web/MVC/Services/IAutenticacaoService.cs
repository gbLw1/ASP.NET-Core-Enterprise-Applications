using NSE.WebApp.MVC.Models;

namespace MVC.Services;

public interface IAutenticacaoService
{
    Task<string> Login(UsuarioLogin usuario);
    Task<string> Registro(UsuarioRegistro usuario);
}
