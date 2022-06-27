namespace NSE.Carrinho.Model;

public class CarrinhoCliente
{
    internal const int MAX_QUANTIDADE_ITEM = 5;

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

    internal void CalcularValorCarrinho()
    {
        ValorTotal = Itens.Sum(i => i.CalcularValor());
    }

    internal bool CarrinhoItemExistente(CarrinhoItem item)
    {
        return Itens.Any(i => i.ProdutoId == item.ProdutoId);
    }

    internal CarrinhoItem ObterProdutoPorId(Guid produtoId)
    {
        return Itens.FirstOrDefault(p => p.ProdutoId == produtoId);
    }

    internal void AdicionarItem(CarrinhoItem item)
    {
        if (!item.EhValido())
        {
            return;
        }

        item.AssociarCarrinho(Id);

        if (CarrinhoItemExistente(item))
        {
            var itemExistente = ObterProdutoPorId(item.ProdutoId);
            itemExistente.AdicionarUnidades(item.Quantidade);

            item = itemExistente;
            Itens.Remove(itemExistente);
        }

        Itens.Add(item);

        CalcularValorCarrinho();
    }
}
