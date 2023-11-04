using FluentValidation;
using System;

namespace NSE.Clientes.API.Applicaation.Commands.Validations
{
    public class AdicionarEnderecoCommandValidation : AbstractValidator<AdicionarEnderecoCommand>
    {
        public AdicionarEnderecoCommandValidation()
        {
            RuleFor(e => e.Estado)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode está vazio.");

            RuleFor(e => e.Cidade)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode está vazio.");

            RuleFor(e => e.Cep)
                .NotEmpty()
                .WithMessage("O campo {PropertyName} não pode está vazio.");

            RuleFor(e => e.Bairro)
               .NotEmpty()
               .WithMessage("O campo {PropertyName} não pode está vazio.");

            RuleFor(e => e.Logradouro)
               .NotEmpty()
               .WithMessage("O campo {PropertyName} não pode está vazio.");

            RuleFor(e => e.Numero)
               .NotEmpty()
               .WithMessage("O campo {PropertyName} não pode está vazio.");

            RuleFor(e => e.ClienteId)
                .NotEmpty()
                .NotEqual(Guid.Empty)
                .WithMessage("O campo {PropertyName} não pode está vazio.");


        }
    }
}
