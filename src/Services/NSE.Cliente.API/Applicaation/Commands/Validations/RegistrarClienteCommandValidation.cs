using FluentValidation;
using NSE.Core.DomainObjects;
using System;

namespace NSE.Clientes.API.Applicaation.Commands.Validations
{
    public class RegistrarClienteCommandValidation : AbstractValidator<RegistrarClienteCommand>
    {
        public RegistrarClienteCommandValidation()
        {
            RuleFor(c => c.Email)
                .Must(TerEmailValido)
                .WithMessage("Verifique o campo {PropertyName}");

            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("O campo {PropertyName} não pode está vazio.");

            RuleFor(c => c.Nome)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode está vazio.");

            RuleFor(c => c.Cpf)
                .Must(TerCpfValido)
                .WithMessage("O campo {PropertyName} não pode está vazio.");
        }

        protected static bool TerCpfValido(string cpf)
        {
            return Cpf.Validar(cpf);
        }

        protected static bool TerEmailValido(string email)
        {
            return Email.Validar(email);
        }
    }
}
