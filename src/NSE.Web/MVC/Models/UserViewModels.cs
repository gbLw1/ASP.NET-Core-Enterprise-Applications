using System.ComponentModel.DataAnnotations;

namespace NSE.WebApp.MVC.Models;

public class UsuarioRegistro
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [MinLength(6, ErrorMessage = "O campo precisa ter no mínimo {1} caracteres")]
    [MaxLength(100, ErrorMessage = "O campo precisa ter no máximo {1} caracteres")]
    public string? Senha { get; set; }

    [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
    public string? SenhaConfirmacao { get; set; }
}

public class UsuarioLogin
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [MinLength(6, ErrorMessage = "O campo precisa ter no mínimo {1} caracteres")]
    [MaxLength(100, ErrorMessage = "O campo precisa ter no máximo {1} caracteres")]
    public string? Senha { get; set; }
}

public class UsuarioRespostaLogin
{
    public string? AccessToken { get; set; }
    public double ExpiresIn { get; set; }
    public UsuarioToken? UsuarioToken { get; set; }
    public ResponseResult? ResponseResult { get; set; }
}

public class UsuarioToken
{
    public string? Id { get; set; }
    public string? Email { get; set; }
    public IEnumerable<UsuarioClaim>? Claims { get; set; }
}

public class UsuarioClaim
{
    public string? Value { get; set; }
    public string? Type { get; set; }
}