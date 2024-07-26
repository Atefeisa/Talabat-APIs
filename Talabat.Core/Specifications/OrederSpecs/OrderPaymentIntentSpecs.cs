using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Specifications.OrederSpecs
{
	public class OrderPaymentIntentSpecs : BaseSpecifications<Order>
	{

        public OrderPaymentIntentSpecs( string PaymentIntentId ) : base( O => O.PaymentIntentId == PaymentIntentId)
        {
            
        }

    }
}
