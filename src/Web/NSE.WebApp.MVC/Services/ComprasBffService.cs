﻿using Microsoft.Extensions.Options;
using NSE.Core.Comunication;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public class ComprasBffService : Service, IComprasBffService
    {

        private readonly HttpClient _httpClient;

        public ComprasBffService(HttpClient httpClient,
                               IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ComprasBffUrl);
        }

        #region Carrinho

        public async Task<CarrinhoViewModel> ObterCarrinho()
        {
            var response = await _httpClient.GetAsync($"compras/carrinho");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<CarrinhoViewModel>(response);
        }


        public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel produto)
        {
            var itemContent = ObterConteudo(produto);

            var response = await _httpClient.PostAsync($"compras/carrinho/items", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return new ResponseResult();
        }

        public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel produto)
        {
            var itemContent = ObterConteudo(produto);

            var response = await _httpClient.PutAsync($"compras/carrinho/items/{produtoId}", itemContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return new ResponseResult();
        }

        public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
        {
            var response = await _httpClient.DeleteAsync($"compras/carrinho/items/{produtoId}");

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return new ResponseResult();
        }

        public async Task<int> ObterQuantidadeCarrinho()
        {
            var response = await _httpClient.GetAsync($"compras/carrinho-quantidade");
            TratarErrosResponse(response);
            return await DeserializarObjetoResponse<int>(response);
        }

        public async Task<ResponseResult> AplicarVoucherCarrinho(string voucher)
        {
            var content = ObterConteudo(voucher);

            var response = await _httpClient.PostAsync($"compras/carrinho/aplicar-voucher", content);

            if (TratarErrosResponse(response) || response.StatusCode == HttpStatusCode.BadRequest) return await DeserializarObjetoResponse<ResponseResult>(response);

            return new ResponseResult();
        }

        #endregion 

        #region Pedido

        public PedidoTransacaoViewModel MapearParaPedido(CarrinhoViewModel carrinho, EnderecoViewModel endereco)
        {
            var pedido = new PedidoTransacaoViewModel
            {
                ValorTotal = carrinho.ValorTotal,
                Itens = carrinho.Itens,
                Desconto = carrinho.Desconto,
                VoucherUtilizado = carrinho.VoucherUtilizado,
                VoucherCodigo = carrinho.Voucher?.Codigo
            };

            if (endereco != null)
            {
                pedido.Endereco = new EnderecoViewModel
                {
                    Logradouro = endereco.Logradouro,
                    Numero = endereco.Numero,
                    Bairro = endereco.Bairro,
                    Cep = endereco.Cep,
                    Complemento = endereco.Complemento,
                    Cidade = endereco.Cidade,
                    Estado = endereco.Estado
                };
            }

            return pedido;
        }

        public async Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao)
        {
            var pedidoContent = ObterConteudo(pedidoTransacao);

            var response = await _httpClient.PostAsync("compras/pedido/", pedidoContent);

            if (!TratarErrosResponse(response)) return await DeserializarObjetoResponse<ResponseResult>(response);

            return new ResponseResult();
        }
        public async Task<PedidoViewModel> ObterUltimoPedido()
        {
            var response = await _httpClient.GetAsync("compras/pedido/ultimo/");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<PedidoViewModel>(response);
        }

        public async Task<IEnumerable<PedidoViewModel>> ObterListaPorClienteId()
        {
            var response = await _httpClient.GetAsync("compras/pedido/lista-cliente/");

            TratarErrosResponse(response);

            return await DeserializarObjetoResponse<IEnumerable<PedidoViewModel>>(response);
        }

        #endregion
    }
}
