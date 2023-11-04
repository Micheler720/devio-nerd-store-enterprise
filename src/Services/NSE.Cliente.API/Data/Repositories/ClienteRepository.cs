using Microsoft.EntityFrameworkCore;
using NSE.Clientes.API.Models;
using NSE.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Clientes.API.Data.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ClienteContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public ClienteRepository(ClienteContext context)
        {
            _context = context;
        }
        public void Adicionar(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
        }
        public async Task<Cliente> ObterPorCpf(string cpf)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Cpf.Numero == cpf);
        }

        public async Task<IEnumerable<Cliente>> ObterTodos()
        {
            return await _context.Clientes.AsNoTracking().ToListAsync();
        }

        public async Task<Endereco> ObterEnderecoPorId(Guid userId)
        {
            return await _context.Enderecos.FirstOrDefaultAsync(c => c.ClienteId == userId);
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
}
