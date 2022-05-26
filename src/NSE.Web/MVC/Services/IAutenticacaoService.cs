using NSE.WebApp.MVC.Models;

namespace MVC.Services;

public interface IAutenticacaoService
{
    Task<UsuarioRespostaLogin> Login(UsuarioLogin usuario);
    Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuario);
}
