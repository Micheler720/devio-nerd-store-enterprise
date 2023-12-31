﻿using NSE.Pedidos.API.Application.DTO;
using System.Threading.Tasks;
using NSE.Pedidos.Domain.Vouchers;

namespace NSE.Pedidos.API.Application.Queries
{
    public class VoucherQueries : IVoucherQueries
    {
        private readonly IVoucherRepository _voucherRepository;

        public VoucherQueries(IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
        }

        public async Task<VoucherDTO> ObterVoucherPorCodigo(string codigo)
        {
            var voucher = await _voucherRepository.ObterVoucherPorCodigo(codigo);
            if (voucher == null) return null;

            if(!voucher.EstaValidoParaUtilizacao()) return null;

            return new VoucherDTO
            {
                Codigo = voucher.Codigo,
                Percentual = voucher.Percentual,
                TipoDesconto = (int)voucher.TipoDesconto,
                ValorDesconto = voucher.ValorDesconto
            };
        }
    }

    public interface IVoucherQueries
    {
        Task<VoucherDTO> ObterVoucherPorCodigo(string codigo);

    }
}
