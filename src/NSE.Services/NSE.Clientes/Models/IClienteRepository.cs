using Core.Data;

namespace NSE.Clientes.Models;

public interface IClienteRepository : IRepository<Cliente>
{
    void Adicionar(Cliente cliente);

    Task<IEnumerable<Cliente>> ObterTodos();
    Task<Cliente?> ObterPorCpf(string cpf);
}
