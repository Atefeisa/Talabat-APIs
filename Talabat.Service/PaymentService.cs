using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Services.Interfaces;
using Talabat.Core.Specifications.OrederSpecs;
using Talabat.Core.UnitOfWork.Interfaces;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Service
{
	public class PaymentService : IPaymentService
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IConfiguration _configuration;

		public PaymentService( IBasketRepository basketRepository ,
			                   IUnitOfWork unitOfWork ,
			                   IConfiguration configuration
			                 ) 
		{

			_basketRepository = basketRepository;
			_unitOfWork = unitOfWork;
			_configuration = configuration;
		
		}



		public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
		{


			//   1  -  Call Stripe

			StripeConfiguration.ApiKey = _configuration["StripeKeys:SecretKey"];




			//  2 -  Get Basket 

			var Basket = await _basketRepository.GetBasketAsync(basketId);
		
			if (Basket is null)    return null ;



			//  3  -  Delivery Method


			var ShippingPrice = 0M;

			if (Basket.DeliveryMethodId.HasValue)
			{

				var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);

				ShippingPrice = DeliveryMethod.Cost;
			}





			//  4  - Total Price  = SubTotal + DM .Cost

			if (Basket.Items.Count > 0)
			{

				foreach (var item in Basket.Items)
				{
					var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

					if (item.Price != Product.Price)
					{

						item.Price = Product.Price;

					}


				}


			}





			//  5 - Sub Total 

				var SubTotal = Basket.Items.Sum( I => I.Price * I.Quantity );






			//   6  -   Create PaymentIntentId


			var Service = new PaymentIntentService();

			PaymentIntent paymentIntent;


            if (string.IsNullOrEmpty(Basket.PaymentIntentId))  // Create new PaymentIntentId
			{


				var Options = new PaymentIntentCreateOptions()
				{
					Amount = ( long ) (  SubTotal  *  100 +  ShippingPrice  *  100 ),

					Currency = "usd",

					PaymentMethodTypes = new List<string>() {"card"}


				};


				paymentIntent = await Service.CreateAsync(Options);
				
				Basket.PaymentIntentId = paymentIntent.Id;

				Basket.CLientSecret = paymentIntent.ClientSecret;

			}
			else   // Update PaymentIntentId
			{

				

				var Options = new PaymentIntentUpdateOptions()
				{

					Amount = ( long ) ( SubTotal  *  100  +  ShippingPrice * 100 ),

				};

				paymentIntent = await Service.UpdateAsync(Basket.PaymentIntentId, Options);

				Basket.PaymentIntentId = paymentIntent.Id;

				Basket.CLientSecret = paymentIntent.ClientSecret;

			}





			//  7  -  Return Basket Included  PaymentIntentId And Client   ( Client Secret )

			await _basketRepository.UpdateBasketAsync(Basket);

			return Basket;
			

		}





		public async Task<Order> UpdatePaymentIntentToSuccessOrFailed(string paymentIntentId, bool flag)
		{


			var spec = new OrderPaymentIntentSpecs(paymentIntentId);


			var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);


            if (flag)
            {

				order.Status = OrderStatus.PaymentReceived;
            
			}
			else
			{
			
				order.Status = OrderStatus.PaymentFailed;
			
			}


			_unitOfWork.Repository<Order>().Update(order);

			await _unitOfWork.CompleteAsync();

			return order;	

        }



	}
}
