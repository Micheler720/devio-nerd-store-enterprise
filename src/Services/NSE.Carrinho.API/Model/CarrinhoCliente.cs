using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSE.Carrinho.API.Model
{
    public class CarrinhoCliente
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public string Cliente { get; set; }
        public decimal ValorTotal { get; set; }
        public List<CarrinhoItem> Itens { get; set; } = new List<CarrinhoItem>();
        public ValidationResult ValidationResult {get; set;}
        public bool VoucherUtilizado {get; set;}
        public decimal Desconto { get; set; }
        public Voucher Voucher { get; set; }
        public CarrinhoCliente(Guid clienteId, string cliente)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
            Cliente   = cliente;
        }
        public CarrinhoCliente()
        {

        }

        internal void AdicionarItem(CarrinhoItem item)
        {
            item.AssociarCarrinho(Id);

            if (CarrinhoItemExistente(item))
            {
                var itemExistente = ObterProdutoPorId(item.ProdutoId);
                itemExistente.AdicionarItem(item.Quantidade);

                item = itemExistente;
                Itens.Remove(itemExistente);
            }

            Itens.Add(item);
            CalcularValorTotal();
        }

        internal void RemoverItem(CarrinhoItem item)
        {
            Itens.Remove(ObterProdutoPorId(item.ProdutoId));
            CalcularValorTotal();
        }

        internal void AtualizarItem(CarrinhoItem item)
        {
            item.AssociarCarrinho(Id);

            var itemExistente = ObterProdutoPorId(item.ProdutoId);

            if (itemExistente != null)
            {
                Itens.Remove(itemExistente);
                Itens.Add(item);
                CalcularValorTotal();
            }
        }


        internal void AtualizarUnidades(CarrinhoItem item, int unidades)
        {
            item.AtualizarUnidade(unidades);
            AtualizarItem(item);
        }



        internal CarrinhoItem ObterProdutoPorId(Guid produtoId)
        {
            return Itens.Find(i => i.ProdutoId == produtoId);
        }

        internal bool CarrinhoItemExistente(CarrinhoItem item)
        {
            return Itens.Any(i => i.Id == item.Id);
        }

        internal void CalcularValorTotal()
        {
            ValorTotal = Itens.Sum(i => i.CalcularValor());
            CalcularValorTotalDesconto();
        }

        internal void AplicarVoucher(Voucher voucher)
        {
            Voucher = voucher;
            VoucherUtilizado = true;
            CalcularValorTotal();
        }

        internal bool EhValido()
        {
            var erros = Itens.SelectMany(i => new CarrinhoItem.ItemCarrinhoValidation().Validate(i).Errors).ToList();
            erros.AddRange(new CarrinhoClienteValidation().Validate(this).Errors.ToList());
            ValidationResult = new ValidationResult(erros);

            return ValidationResult.IsValid;
        }

        private void CalcularValorTotalDesconto()
        {
            if(!VoucherUtilizado) return;

            decimal desconto = 0;
            var valor = ValorTotal;

            if(Voucher.TipoDesconto == TipoDescontoVoucher.Porcentagem)
            {
                if(Voucher.Percentual.HasValue)
                {
                    desconto = (valor * Voucher.Percentual.Value) / 100;
                    valor -= desconto;
                }
            }
            else
            {
                if(Voucher.ValorDesconto.HasValue)
                {
                    desconto = Voucher.ValorDesconto.Value;
                    valor -= desconto;
                }
            }

            ValorTotal = valor < 0 ? 0 : valor;
            Desconto = desconto;
        }


        public class CarrinhoClienteValidation: AbstractValidator<CarrinhoCliente>
        {
            public CarrinhoClienteValidation()
            {
                RuleFor(c => c.ClienteId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Cliente não reconhecido");

                RuleFor(c => c.Itens.Count)
                    .GreaterThan(0)
                    .WithMessage("O carrinho não possui itens");

                RuleFor(c => c.ValorTotal)
                    .GreaterThan(0)
                    .WithMessage("O valor total do carrinho precisa ser maior que 0");
            }
        }
    }
}
