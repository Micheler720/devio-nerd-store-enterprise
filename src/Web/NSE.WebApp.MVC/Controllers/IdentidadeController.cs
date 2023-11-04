using Microsoft.AspNetCore.Mvc;
using NSE.WebApp.MVC.Services;
using System.Threading.Tasks;
using static NSE.WebApp.MVC.Models.UsuarioViewModel;

namespace NSE.WebApp.MVC.Controllers
{
    public class IdentidadeController : MainController
    {

        private readonly IAutenticacaoService _autenticacaoService;

        public IdentidadeController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpGet("nova-conta")]
       public IActionResult Registro()
       {
            return View();
       }

        [HttpPost("nova-conta")]
        public async Task<IActionResult> Registro(UsuarioRegistro usuario)
        {
            if (!ModelState.IsValid) return View(usuario);

            var registro = await _autenticacaoService.Registro(usuario);

            if(ResponsePossuiErros(registro.ResponseResult)) return View(usuario);

            //realizar o login
            await _autenticacaoService.RealizarLogin(registro);

            return RedirectToAction("Index","Home");
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UsuarioLogin usuario, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(usuario);
            var autenticacao = await _autenticacaoService.Login(usuario);

            if (ResponsePossuiErros(autenticacao.ResponseResult)) return View(usuario);

            //realizar o login
            await _autenticacaoService.RealizarLogin(autenticacao);

            if(string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");

            return LocalRedirect(returnUrl);
        }

        [HttpGet("sair")]
        public async Task<IActionResult> Logout()
        {
            await _autenticacaoService.Logout();
            return RedirectToAction("Index", "Home");
        }

      
        
    }
}
