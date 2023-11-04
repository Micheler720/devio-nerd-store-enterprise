using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Services;
using System;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Controllers
{
    public class CatalogoController : MainController
    {
        private readonly ICatalogoService _catalogoService;
        public CatalogoController(ICatalogoService catalogoService)
        {
            _catalogoService = catalogoService;
        }

        [HttpGet("")]
        [Route("vitrine")]
        public async Task<IActionResult> Index(
            [FromQuery] int pageSize = 8,
            [FromQuery] int page = 1,
            [FromQuery] string q = null)
        {
            var produtos = await _catalogoService.ObterTodos(pageSize, page, q);
            ViewBag.Pesquisa = q;
            produtos.ReferenceAction = "Index";
            return View(produtos);
        }

        [HttpGet("produto-detalhe/{id}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            var produto = await _catalogoService.ObterPorId(id);
            return View(produto);
        }
    }
}
