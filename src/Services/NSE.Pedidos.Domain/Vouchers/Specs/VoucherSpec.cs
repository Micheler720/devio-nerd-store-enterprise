using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NSE.Core.Specification;
using NSE.Pedidos.Domain.Vouchers;

namespace NSE.Pedidos.Domain.Vouchers.Specs
{
    public class VoucherDataSpecification : Specification<Voucher>
    {
        public VoucherDataSpecification()
        {
        }
        public override Expression<Func<Voucher, bool>> ToExpression()
        {
            return voucher => voucher.DataValidade >= DateTime.Now.Date;
        }
    }

    public class VoucherQuantidadeSpecification : Specification<Voucher>
    {
        public VoucherQuantidadeSpecification()
        {
        }
        public override Expression<Func<Voucher, bool>> ToExpression()
        {
            return voucher => voucher.Quantidade > 0;
        }
    }

    public class VoucherAtivoSpecification : Specification<Voucher>
    {
        public VoucherAtivoSpecification()
        {
        }
        public override Expression<Func<Voucher, bool>> ToExpression()
        {
            return voucher => voucher.Ativo && !voucher.Utilizado;
        }
    }
}
