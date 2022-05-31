namespace NSE.WebAPI.Core.Identidade;

public class AppSettings
{
    public string Secret { get; set; } = default!;
    public int ExpiracaoHoras { get; set; }
    public string Emissor { get; set; } = default!;
    public string ValidoEm { get; set; } = default!;
}
