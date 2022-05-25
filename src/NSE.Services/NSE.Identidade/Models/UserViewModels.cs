using System.ComponentModel.DataAnnotations;

namespace NSE.Identidade.Models;

public class UsuarioRegistro
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1] caracteres", MinimumLength = 6)]
    public string Senha { get; set; } = default!;

    [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
    public string SenhaConfirmacao { get; set; } = default!;
}

public class UsuarioLogin
{
    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "O campo {0} é obrigatório")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1] caracteres", MinimumLength = 6)]
    public string Senha { get; set; } = default!;
}

public class UsuarioRespostaLogin
{
    public string AccessToken { get; set; } = default!;
    public double ExpiresIn { get; set; }
    public UsuarioToken UsuarioToken { get; set; } = default!;
}

public class UsuarioToken
{
    public string Id { get; set; } = default!;
    public string Email { get; set; } = default!;
    public IEnumerable<UsuarioClaim> Claims { get; set; } = default!;
}

public class UsuarioClaim
{
    public string Value { get; set; } = default!;
    public string Type { get; set; } = default!;
}