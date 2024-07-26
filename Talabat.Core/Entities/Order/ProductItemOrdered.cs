using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order
{
    public class ProductItemOrdered
    {
		public ProductItemOrdered(
			                      int productId, 
			                      string productName,
			                      string picturetUrl
								 )

		{

			ProductId = productId;
			ProductName = productName;
			PicturetUrl = picturetUrl;

		}


		public int ProductId { get; set; }
        public string ProductName { get; set; }
		public string PicturetUrl { get; set; }

	}
}
