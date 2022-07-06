namespace NSE.Carrinho.Model;

public class Voucher
{
    public string? Codigo { get; set; }
    public decimal? Percentual { get; set; }
    public decimal? ValorDesconto { get; set; }
    public TipoDesconto TipoDesconto { get; set; }
}

public enum TipoDesconto
{
    Porcentagem = 0,
    Valor = 1
}