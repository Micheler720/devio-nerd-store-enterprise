using FluentValidation.Results;
using MediatR;
using NSE.Clientes.API.Applicaation.Events;
using NSE.Clientes.API.Models;
using NSE.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Clientes.API.Applicaation.Commands
{
    public class ClienteCommandHandler : CommandHandler,
        IRequestHandler<RegistrarClienteCommand, ValidationResult>,
        IRequestHandler<AdicionarEnderecoCommand, ValidationResult>
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteCommandHandler(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
        {

            if (!message.EhValido()) return message.ValidationResult;

            var cliente = new Cliente(message.Id, message.Nome, message.Email, message.Cpf);
            var clienteExistente = await _clienteRepository.ObterPorCpf(cliente.Cpf.Numero);

            if(clienteExistente != null)
            {
                AdicionarErro("Este CPF já está em uso.");
                return ValidationResult;
            }

            _clienteRepository.Adicionar(cliente);

            cliente.AdicionarEvento(new ClienteRegistradoEvent(cliente.Id, cliente.Nome, cliente.Email.Endereco, cliente.Cpf.Numero));
            return await PersistirDados(_clienteRepository.UnitOfWork); 
        }

        public async Task<ValidationResult> Handle(AdicionarEnderecoCommand message, CancellationToken cancellationToken)
        {
            if (!message.EhValido()) return message.ValidationResult;


            _clienteRepository.AdicionarEndereco(MapearParaEndereco(message));

            var result = await PersistirDados(_clienteRepository.UnitOfWork);
            return result;
        }

        private Endereco MapearParaEndereco(AdicionarEnderecoCommand enderecoCommand)
        {
            return new Endereco(
                enderecoCommand.Logradouro,
                enderecoCommand.Numero,
                enderecoCommand.Complemento,
                enderecoCommand.Bairro,
                enderecoCommand.Cep,
                enderecoCommand.Cidade,
                enderecoCommand.Estado,
                enderecoCommand.ClienteId);
           
        }
    }
}
