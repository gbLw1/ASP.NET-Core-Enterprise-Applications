using System.ComponentModel.DataAnnotations;

namespace NSE.WebApp.MVC.Models;

public class UsuarioRegistro
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [MinLength(6, ErrorMessage = "O campo precisa ter no mínimo {1} caracteres")]
    [MaxLength(100, ErrorMessage = "O campo precisa ter no máximo {1} caracteres")]
    public string Senha { get; set; } = string.Empty;

    [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
    public string SenhaConfirmacao { get; set; } = string.Empty;
}

public class UsuarioLogin
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [MinLength(6, ErrorMessage = "O campo precisa ter no mínimo {1} caracteres")]
    [MaxLength(100, ErrorMessage = "O campo precisa ter no máximo {1} caracteres")]
    public string Senha { get; set; } = string.Empty;
}
