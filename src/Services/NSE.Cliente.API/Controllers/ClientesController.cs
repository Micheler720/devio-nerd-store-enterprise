﻿using Microsoft.AspNetCore.Mvc;
using NSE.Clientes.API.Applicaation.Commands;
using NSE.Clientes.API.Models;
using NSE.Core.Mediator;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Usuario;
using System;
using System.Threading.Tasks;

namespace NSE.Clientes.API.Controllers
{
    [Route("api/cliente")]
    public class ClientesController : MainController
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IClienteRepository _clienteRepository;
        private readonly IAspNetUser _user;
        public ClientesController(IMediatorHandler mediatorHandler,
            IClienteRepository clienteRepository,
            IAspNetUser user)
        {
            _mediatorHandler = mediatorHandler;
            _clienteRepository = clienteRepository;
            _user = user;
        }

        [HttpGet("endereco")]
        public async Task<IActionResult> ObterEndereco()
        {
            var endereco = await _clienteRepository.ObterEnderecoPorId(_user.ObterUserId());

           return endereco == null ? NotFound() : CustomResponse(endereco);
        }

        [HttpPost("endereco")]
        public async Task<IActionResult> AdicionarEndereco(AdicionarEnderecoCommand endereco)
        {
            endereco.ClienteId = _user.ObterUserId();

            return CustomResponse(await _mediatorHandler.EnviarComado(endereco));
        }
    }
}
