using FluentValidation;
using System;
using System.Text.Json.Serialization;

namespace NSE.Carrinho.API.Model
{
    public class CarrinhoItem
    {
        public Guid    Id { get; set; }
        public Guid    ProdutoId { get; set; }
        public string  Nome { get; set; }
        public int     Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string  Imagem { get; set; }
        public Guid    CarrinhoId { get; set; }

        [JsonIgnore]
        public CarrinhoCliente CarrinhoCliente { get; set; }
        public CarrinhoItem()
        {
            Id = Guid.NewGuid();
        }

        internal void AssociarCarrinho(Guid carrinhoId)
        {
            CarrinhoId = carrinhoId;
        }

        internal decimal CalcularValor()
        {
            return Valor * Quantidade;
        }

        internal void AdicionarItem(int quantidade)
        {
            Quantidade += quantidade;
        }

        internal void AtualizarUnidade(int unidades)
        {
            this.Quantidade = unidades;
        }

        internal bool EhValido()
        {
            return new ItemCarrinhoValidation().Validate(this).IsValid;
        }

        public class ItemCarrinhoValidation : AbstractValidator<CarrinhoItem>
        {

            public ItemCarrinhoValidation()
            {
                RuleFor(i => i.ProdutoId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("O Id do produto não pode ser vazio.");

                RuleFor(c => c.Nome)
                    .NotEmpty()
                    .WithMessage("O nome do produto não foi informado.");

                RuleFor(c => c.Quantidade)
                    .GreaterThan(0)
                    .WithMessage(item => $"A quantidade minima do item {item.Nome} é 1.");

                RuleFor(c => c.Quantidade)
                    .LessThan(6)
                    .WithMessage(item => $"A quantidade maxima do item {item.Nome} é 5.");

                RuleFor(c => c.Valor)
                    .GreaterThan(0)
                    .WithMessage((item => $"O valor do item {item.Nome} precisa ser maior que 0."));
            }
        
        }

    }
}
