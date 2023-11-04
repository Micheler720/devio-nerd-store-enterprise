using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Bff.Compras.Models;
using NSE.Bff.Compras.Services;
using NSE.Bff.Compras.Services.gRPC;
using NSE.WebAPI.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Bff.Compras.Controllers
{
    [Route("api/compras")]
    public class CarrinhoController : MainController
    {
        private readonly ICatalogoService _catalogoService;
        private readonly ICarrinhoService _carrinhoService;
        private readonly IPedidoService _pedidoService;
        private readonly ICarrinhoGrpcService _carrinhoGrpcService;

        public CarrinhoController(ICatalogoService catalogoService,
                                  ICarrinhoService carrinhoService,
                                  IPedidoService pedidoService,
                                  ICarrinhoGrpcService carrinhoGrpcService)
        {
            _catalogoService = catalogoService;
            _carrinhoService = carrinhoService;
            _pedidoService = pedidoService;
            _carrinhoGrpcService = carrinhoGrpcService;
        }

        [HttpGet]
        [Route("carrinho")]
        public async Task<IActionResult> Index()
        {
            return CustomResponse(await _carrinhoGrpcService.ObterCarrinho());
        }

        [HttpGet]
        [Route("carrinho-quantidade")]
        public async Task<int> ObterQuantidadeCarrinho()
        {
            var quantidade = await _carrinhoGrpcService.ObterCarrinho();
            return quantidade?.Itens.Sum(i => i.Quantidade) ?? 0;
        }

        [HttpPost]
        [Route("carrinho/items")]
        public async Task<IActionResult> AdicionarItemCarrinho([FromBody]ItemCarrinhoDTO itemProduto)
        {
            var produto = await _catalogoService.ObterPorId(itemProduto.ProdutoId);
            await ValidarItemCarrinho(produto, itemProduto.Quantidade);

            if (!OperacaoValida()) return CustomResponse();

            itemProduto.Nome = produto.Nome;
            itemProduto.Valor = produto.Valor;
            itemProduto.Imagem = produto.Imagem;

            return CustomResponse(await _carrinhoService.AdicionarItemCarrinho(itemProduto));
        }

        [HttpPut]
        [Route("carrinho/items/{produtoId:Guid}")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, [FromBody] ItemCarrinhoDTO itemProduto )
        {
            var produto = await _catalogoService.ObterPorId(produtoId);
            await ValidarItemCarrinho(produto, itemProduto.Quantidade);

            if (!OperacaoValida()) return CustomResponse();
            return CustomResponse(await _carrinhoService.AtualizarItemCarrinho(produtoId, itemProduto));
        }

        [HttpDelete]
        [Route("carrinho/items/{produtoId:Guid}")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var produto = await _catalogoService.ObterPorId(produtoId);

            if(produto == null)
            {
                AdicionarErroProcessamento("Produto Inesxistente");
                return CustomResponse();
            }

            return CustomResponse(await _carrinhoService.RemoverItemCarrinho(produtoId));
        }

        [HttpPost("carrinho/aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher([FromBody] string voucherCodigo)
        {
            var voucher = await _pedidoService.ObterVoucherPorCodigo(voucherCodigo);

            if(voucher is null)
            {
                AdicionarErroProcessamento("Voucher invalido ou Inesxistente");
                return CustomResponse();
            }
            await _carrinhoService.AplicarVoucherCarrinho(voucher);

            return CustomResponse(voucher);
        }


        private async Task ValidarItemCarrinho(ItemProdutoDTO produto, int quantidade, Guid produtoId = default)
        {
            if (produto == null) AdicionarErroProcessamento("Produto Inexistente");
            if (quantidade < 1)  AdicionarErroProcessamento($"Escolha ao menos uma unidade do produto {produto.Nome}");

            var carrinho = await _carrinhoService.ObterCarrinho();
            var itemCarrinho = carrinho.Itens.FirstOrDefault(p => p.ProdutoId == produto.Id);

            if(itemCarrinho != null && itemCarrinho.Quantidade + quantidade > produto.QuantidadeEstoque)
            {
                AdicionarErroProcessamento($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}");
                return;
            }

            if (quantidade > produto.QuantidadeEstoque)
            {
                AdicionarErroProcessamento($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}.");
            }
        }
    }
}
