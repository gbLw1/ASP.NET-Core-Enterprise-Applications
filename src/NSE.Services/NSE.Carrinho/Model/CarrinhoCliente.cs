namespace NSE.Carrinho.Model;

public class CarrinhoCliente
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public List<CarrinhoItem> Itens { get; set; } = new List<CarrinhoItem>();

    public CarrinhoCliente() { }

    public CarrinhoCliente(Guid clienteId)
    {
        Id = Guid.NewGuid();
        ClienteId = clienteId;
    }
}
