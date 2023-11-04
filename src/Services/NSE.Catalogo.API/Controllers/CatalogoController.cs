using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Catalogo.API.Models;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Catalogo.API.Controllers
{
    [Authorize]
    public class CatalogoController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;

        public CatalogoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [AllowAnonymous]
        [HttpGet("catalogo/produtos")]
        public async Task<PagedResult<Produto>> Index(
            [FromQuery] int pageSize = 8,
            [FromQuery] int page = 1,
            [FromQuery] string q = null)
        {
            return await _produtoRepository.ObterTodos(pageSize, page, q);
        }

        [ClaimsAuthorize("Catalogo", "Ler")]
        [HttpGet("catalogo/produtos/{id}")]
        public async Task<ActionResult<Produto>> ProdutoDetalhe(Guid id)
        {

            return Ok(await _produtoRepository.ObterPorId(id));
        }

        [ClaimsAuthorize("Catalogo", "Ler")]
        [HttpGet("catalogo/produtos/lista/{ids}")]
        public async Task<ActionResult<Produto>> ProdutosLista(string ids)
        {

            return Ok(await _produtoRepository.ObterProdutosPorIds(ids));
        }
    }
}
