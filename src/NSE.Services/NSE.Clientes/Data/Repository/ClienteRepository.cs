using Core.Data;
using Microsoft.EntityFrameworkCore;
using NSE.Clientes.Models;

namespace NSE.Clientes.Data.Repository;

public class ClienteRepository : IClienteRepository
{
    private readonly ClientesContext _context;

    public ClienteRepository(ClientesContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<IEnumerable<Cliente>> ObterTodos()
        => await _context.Clientes.AsNoTracking().ToListAsync();

    public async Task<Cliente?> ObterPorCpf(string cpf)
        => await _context.Clientes.FirstOrDefaultAsync(c => c.Cpf!.Numero == cpf);

    public void Adicionar(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
    }

    public async Task<Endereco?> ObterEnderecoPorId(Guid clienteId)
    {
        return await _context.Enderecos.FirstOrDefaultAsync(e => e.ClienteId == clienteId);
    }

    public void AdicionarEndereco(Endereco endereco)
    {
        _context.Enderecos.Add(endereco);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
