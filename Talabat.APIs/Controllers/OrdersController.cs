using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Order;
using Talabat.Core.Services.Interfaces;
using Talabat.Core.UnitOfWork.Interfaces;

namespace Talabat.APIs.Controllers
{
	public class OrdersController : ApiBaseController
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;

		public OrdersController(IOrderService orderService , IMapper mapper, IUnitOfWork unitOfWork )
        {
			_orderService = orderService;
			_mapper = mapper;
		    _unitOfWork = unitOfWork;
		}




		#region  1   -  Create Order




		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
		[HttpPost]          //  POST  :  /api/orders
		[Authorize]
		public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto model)
		{

			var BuyerEmail   = User.FindFirstValue(ClaimTypes.Email);

			var MappdAddress = _mapper.Map<AddressDto, Address>(model.ShipToAddress);

			var Order = await _orderService.CreateOrdersAsync(BuyerEmail, model.BasketId, model.DeliveryMethodId, MappdAddress);


			if (Order is null)
				return BadRequest(new ApiResponse(400, " There is a problem With your order ! "));

			var Result = _mapper.Map<Order, OrderToReturnDto>(Order);

			return Ok(Result);
		}





		#endregion




		#region  2   - Get Order for Users 






		[ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet]          //  GET  :  /api/orders
		[Authorize]
		public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
		{

			var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);


			var Order = await _orderService.GetOrdersForSpecificUserAsync(BuyerEmail);


			if (Order is null)
				return NotFound(new ApiResponse(404, " There is no orders for you ! "));


			var Result = _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(Order);



			return Ok(Result);

		}



		#endregion




		#region  3   - Get Order By Id for User 



		[ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]       //  GET  :  /api/orders/1
		[Authorize]
		public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
		{

			var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);


			var Order = await _orderService.GetOrdersByIdForSpecificUserAsync(BuyerEmail, id);


			if (Order is null)
				return NotFound(new ApiResponse(404, $" There Is Not Order With Id : {id} , For You ! "));


			var Result = _mapper.Map<Order, OrderToReturnDto>(Order);



			return Ok(Result);


		}


		#endregion



		#region  4   - Delivery Method



		[HttpGet("DeliveryMethods")]       //  GET  :  /api/orders/DeliveryMethods

		public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
		{
			var deliveyMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

			return Ok(deliveyMethod);
		}



		#endregion

















	}


}
