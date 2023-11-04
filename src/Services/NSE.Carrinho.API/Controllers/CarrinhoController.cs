using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSE.Carrinho.API.Data;
using NSE.Carrinho.API.Model;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Usuario;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Carrinho.API.Controllers
{
    [Route("api/carrinho")]
    [Authorize]
    public class CarrinhoController : MainController
    {

        private readonly IAspNetUser _aspNetUser;
        private readonly CarrinhoContext _context;

        public CarrinhoController(IAspNetUser aspNetUser, CarrinhoContext context)
        {
            _aspNetUser = aspNetUser;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObterCarrinho()
        {
            return CustomResponse(await CarrinhoCliente() ?? new CarrinhoCliente(_aspNetUser.ObterUserId(), _aspNetUser.ObterUserEmail()));
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarItemCarrinho([FromBody]CarrinhoItem item)
        {
            var carrinho = await CarrinhoCliente();

            if(carrinho == null)
                ManipularNovoCarrinho(item);
            else
                ManipularCarrinhoItemExistente(carrinho, item);
            
            if (!OperacaoValida()) return CustomResponse();

            await PersistirDados();

            return CustomResponse();
        }

        [HttpPut("{produtoId:Guid}")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, CarrinhoItem item)
        {
            var carrinho = await CarrinhoCliente();

            var itemCarrinho = await ObterItemCarrinhoValidado(produtoId, carrinho, item);

            if(itemCarrinho == null) return CustomResponse();

            ValidadarCarrinho(carrinho);
            if (!OperacaoValida()) return CustomResponse();

            carrinho.AtualizarUnidades(itemCarrinho, item.Quantidade);

            _context.CarrinhoItens.Update(itemCarrinho);
            _context.CarrinhoCliente.Update(carrinho);

            await PersistirDados();
            return CustomResponse();
        }

        [HttpDelete("{produtoId:Guid}")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var carrinho = await CarrinhoCliente();
            var itemCarrinho = await ObterItemCarrinhoValidado(produtoId, carrinho);

            if (itemCarrinho == null) return CustomResponse();

            ValidadarCarrinho(carrinho);
            if (!OperacaoValida()) return CustomResponse();

            carrinho.RemoverItem(itemCarrinho);

            _context.CarrinhoItens.Remove(itemCarrinho);
            _context.CarrinhoCliente.Update(carrinho);

            await PersistirDados();
            return CustomResponse();
        }

        [HttpPost]
        [Route("aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher(Voucher voucher)
        {
            var carrinho = await CarrinhoCliente();

            carrinho.AplicarVoucher(voucher);

            _context.CarrinhoCliente.Update(carrinho);

            await PersistirDados();
            return CustomResponse();
        }
        private async Task<CarrinhoCliente> CarrinhoCliente()
        {
            var itens =  await _context.CarrinhoCliente
                    .Include(c => c.Itens)
                    .FirstOrDefaultAsync(c => c.ClienteId == _aspNetUser.ObterUserId());

            return itens;
        }

        private void ManipularNovoCarrinho(CarrinhoItem item)
        {
            var carrinho = new CarrinhoCliente(_aspNetUser.ObterUserId(), _aspNetUser.ObterUserEmail());
            carrinho.AdicionarItem(item);
            _context.CarrinhoCliente.Add(carrinho);
            ValidadarCarrinho(carrinho);
        }

        private void ManipularCarrinhoItemExistente(CarrinhoCliente carrinho, CarrinhoItem item)
        {
            var produtoExistente = carrinho.ObterProdutoPorId(item.ProdutoId);
            ValidadarCarrinho(carrinho);
            if (produtoExistente != null)
            {
                carrinho.AtualizarItem(item);
                _context.CarrinhoItens.Remove(produtoExistente);
            }else
            {
                carrinho.AdicionarItem(item);
            }
            _context.CarrinhoItens.Add(item);
            _context.CarrinhoCliente.Update(carrinho);

        }

        private async Task<CarrinhoItem> ObterItemCarrinhoValidado(Guid produtoId, CarrinhoCliente carrinho, CarrinhoItem item = null)
        {
            
            if(item != null)
            {
                if (produtoId != item.ProdutoId)
                {
                    AdicionarErroProcessamento("O item não corresponde ao item informado");
                    return null;
                }
            }

            if (carrinho == null)
            {
                AdicionarErroProcessamento("Carrinho não encontrado.");
                return null;
            }

            var itemCarrinho = await _context.CarrinhoItens.FirstAsync(i => i.ProdutoId == produtoId && i.CarrinhoId == carrinho.Id);

            if (itemCarrinho == null || !carrinho.CarrinhoItemExistente(itemCarrinho))
            {
                AdicionarErroProcessamento("O item não está no carrinho");
                return null;
            }

            return itemCarrinho;

        }

        private async Task PersistirDados()
        {
            
            var result = await _context.SaveChangesAsync();
            if (result <= 0) AdicionarErroProcessamento("Não foi possivel persistir os dados no banco");
        }

        private bool ValidadarCarrinho(CarrinhoCliente carrinho)
        {
            if (carrinho.EhValido()) return true;

            carrinho.ValidationResult.Errors.ToList().ForEach(e => AdicionarErroProcessamento(e.ErrorMessage));
            return false;
        }
    }
}
