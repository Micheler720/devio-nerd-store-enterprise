﻿using NSE.Clientes.API.Applicaation.Commands.Validations;
using NSE.Core.Messages;
using System;

namespace NSE.Clientes.API.Applicaation.Commands
{
    public class AdicionarEnderecoCommand : Command
    {
        public Guid ClienteId { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public AdicionarEnderecoCommand()
        {

        }

        public AdicionarEnderecoCommand(Guid clienteId, string logradouro, string numero, string complemento, string bairro, string cep, string cidade, string estado)
        {
            ClienteId = clienteId;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cep = cep;
            Cidade = cidade;
            Estado = estado;
        }

        public override bool EhValido()
        {
            var validation = new AdicionarEnderecoCommandValidation().Validate(this);
            return validation.IsValid;
        }

    }
}
