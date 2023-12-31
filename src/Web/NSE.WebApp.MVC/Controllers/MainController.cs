﻿using Microsoft.AspNetCore.Mvc;
using NSE.Core.Comunication;
using NSE.WebApp.MVC.Models;
using System.Linq;

namespace NSE.WebApp.MVC.Controllers
{
    public class MainController : Controller
    {
        protected bool ResponsePossuiErros(ResponseResult resposta)
        {
            if(resposta != null)
            {
                if (resposta.Errors.Mensagens.Any())
                {
                    foreach (var mensagem in resposta.Errors.Mensagens)
                    {
                        ModelState.AddModelError(string.Empty, mensagem);
                    }
                    return true;
                }
            }
            return false;
        }

        protected void AdicionarErroValidacao(string mensagem)
        {
            ModelState.AddModelError(string.Empty, mensagem);
        }

        protected bool OperacaoValida()
        {
            return ModelState.ErrorCount == 0;
        }
    }
}
