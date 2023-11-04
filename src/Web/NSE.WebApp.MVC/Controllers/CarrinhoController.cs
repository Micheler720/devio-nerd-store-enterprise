using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Models;
using NSE.WebApp.MVC.Services;
using System;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class CarrinhoController : MainController
    {
        readonly IComprasBffService _service;

        public CarrinhoController(IComprasBffService service)
        {
            _service         = service;
        }

        [Route("carrinho")]
        public async Task<IActionResult> Index()
        {
            return View(await _service.ObterCarrinho());
        }

        [HttpPost]
        [Route("carrinho/adicionar-item")]
        public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoViewModel item)
        {

            var response = await _service.AdicionarItemCarrinho(item);
            if (ResponsePossuiErros(response)) return View("Index", await _service.ObterCarrinho());
            return RedirectToAction("Index");
        }


        [HttpPost]
        [Route("carrinho/atualizar-item")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, int quantidade)
        {
            var item = new ItemCarrinhoViewModel { ProdutoId = produtoId, Quantidade = quantidade };
            var response = await _service.AtualizarItemCarrinho(produtoId, item);

            if (ResponsePossuiErros(response)) return View("Index", await _service.ObterCarrinho());
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("carrinho/remover-item")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {

            var response = await _service.RemoverItemCarrinho(produtoId);

            if (ResponsePossuiErros(response)) return View("Index", await _service.ObterCarrinho());
            return RedirectToAction("Index");
        }
    
        [HttpPost]
        [Route("carrinho/aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher(string voucherCodigo)
        {
            var resposta = await _service.AplicarVoucherCarrinho(voucherCodigo);

            if (ResponsePossuiErros(resposta)) return View("Index", await _service.ObterCarrinho());

            return RedirectToAction("Index");
        }
    }
}
