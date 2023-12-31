﻿using NSE.Core.Comunication;
using NSE.WebApp.MVC.Models;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
    public interface IClienteService
    {
        Task<ResponseResult> AdicionarEndereco(EnderecoViewModel endereco);
        Task<EnderecoViewModel> ObterEndereco();
    }
}
