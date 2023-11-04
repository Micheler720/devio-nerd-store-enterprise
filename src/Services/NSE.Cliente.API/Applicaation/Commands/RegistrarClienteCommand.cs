using NSE.Clientes.API.Applicaation.Commands.Validations;
using NSE.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Clientes.API.Applicaation.Commands
{
    public class RegistrarClienteCommand : Command
    {
        public Guid   Id { get; private set; }
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; private set; }

        public RegistrarClienteCommand(Guid id, string nome, string email, string cpf)
        {
            AggregateId = Id;
            Id = id;
            Nome = nome;
            Email = email;
            Cpf = cpf;
        }

        public override bool EhValido()
        {
            var validation = new RegistrarClienteCommandValidation().Validate(this);
            return validation.IsValid;
        }
    }
}
