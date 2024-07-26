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

namespace Talabat.Service
{
	public class OrderService : IOrderService
	{

		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IPaymentService _paymentService;

		public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork , IPaymentService paymentService)
		{
			_basketRepository = basketRepository;
			_unitOfWork = unitOfWork;
			_paymentService = paymentService;
		}


		public async Task<Order?> CreateOrdersAsync(string BuyerEmail, string basketId, int DeliveryMethodId, Address ShippingAddress)
		{

		

			//// 1 Get Basket From Basket Repo
			
			var Basket = await _basketRepository.GetBasketAsync(basketId);


			// 2- Get Selected Items From Basket

			 var OrderItems = new List<OrderItem>();


			if (Basket?.Items.Count > 0)
			{
				
				foreach (var item in Basket.Items)
				{

					var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

					var ProductItemOrdered = new ProductItemOrdered(Product.Id, Product.Name, Product.PictureUrl);

					var orderItem = new OrderItem(ProductItemOrdered, item.Price, item.Quantity);

					OrderItems.Add(orderItem);

				}
			}


			// 3. Calculate SubTotal

			var subTotal = OrderItems.Sum(I => I.Price * I.Quantity);


			// 4- Get Delivery Method From Database.

		 	var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);





			//  5  -   Check PaymentIntentId Exists From another Order ?


			var spec = new OrderPaymentIntentSpecs(Basket.PaymentIntentId);

			var EsistOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);



			if (EsistOrder is not null)
			{


				_unitOfWork.Repository<Order>().Delete(EsistOrder);



				// Update Payment Intetnt Id With Amount Of Basket If Changed .

				Basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);



			}






			//   6   -  Create Order

			var Order = new Order( BuyerEmail, ShippingAddress, deliveryMethod , OrderItems , subTotal , Basket.PaymentIntentId);




			//  7    -    Add Order  (  Locally )

			await _unitOfWork.Repository<Order>().AddAsync(Order);





			//    8   -    Save Order to Database ( Remotly )
			 
			var Result = await _unitOfWork.CompleteAsync();

			if (Result <= 0) return null;
			
			return Order;



		}




		public async Task<IReadOnlyList<Order>?> GetOrdersForSpecificUserAsync(string BuyerEmail)
		{

			var specs = new OrderSpecs(BuyerEmail);

			var order = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(specs);

			return order;


		}





		public async Task<Order?> GetOrdersByIdForSpecificUserAsync(string BuyerEmail, int OrderId)
		{
			var spec = new OrderSpecs(BuyerEmail,OrderId);

			var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);

			if (order is null)  return null;

			return order;



		}


	}
}
