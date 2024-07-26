﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order;

namespace Talabat.Core.Specifications.OrederSpecs
{
	public class OrderSpecs : BaseSpecifications<Order>
	{

		public OrderSpecs(string email) : base(O => O.BuyerEmail == email)
		{
			Includes.Add(O => O.DeliveryMethod);

			Includes.Add(O => O.Items);

			AddOrderByDesc(O => O.OrderDate);


		}


        public OrderSpecs(string email , int orderId)
			            : base(O => O.BuyerEmail == email && O.Id == orderId)
        {
			Includes.Add(O => O.DeliveryMethod);

			Includes.Add(O => O.Items);

		}


        








	}
}
