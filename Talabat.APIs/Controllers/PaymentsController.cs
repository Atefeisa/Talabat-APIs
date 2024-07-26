using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Services.Interfaces;

namespace Talabat.APIs.Controllers
{
	[Authorize]
	public class PaymentsController : ApiBaseController
	{
		private readonly IPaymentService _paymentService;
		private readonly IMapper _mapper;

			const string endpointSecret = "whsec_6e2bb0e15852efaecd7de81143d01a52e091b2245cde9acc2c1b963327dcde94";


		public PaymentsController(IPaymentService  paymentService , IMapper mapper )
        {
			_paymentService = paymentService;
			_mapper = mapper;
		}
	
		
		




		[ProducesResponseType( typeof( CustomerBasketDto ) , StatusCodes.Status200OK )]
		[ProducesResponseType( typeof( ApiResponse ) , StatusCodes.Status400BadRequest )]
		//[HttpPost]

		[HttpPost("{basketId}")]
		public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
		{

			var Basket = await _paymentService.CreateOrUpdatePaymentIntent( basketId );

			if (Basket is null)
				return BadRequest(new ApiResponse(400, "There Is A Problem With Your Basket ! ") );

			var MappedBasket = _mapper.Map<CustomerBasketDto>( Basket );

			return Ok( MappedBasket );	
		}




		[AllowAnonymous]
		[HttpPost("webhook")]       // POST  :  /api/payment/webhook
		public async Task<IActionResult> StripWebHook()
		{

			var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();


			try
			{


				var stripeEvent = EventUtility.ConstructEvent(json,
					Request.Headers["Stripe-Signature"], endpointSecret);


				var paymentIntent = stripeEvent.Data.Object as PaymentIntent;




				// Handle the event

				if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
				{

					await _paymentService.UpdatePaymentIntentToSuccessOrFailed(paymentIntent.Id, false);

				}
				else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
				{

					await _paymentService.UpdatePaymentIntentToSuccessOrFailed(paymentIntent.Id, true);


				}

				// ... handle other event types

				else
				{

					Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);

				}

				return Ok();
			}

			catch (StripeException e)
			{

				return BadRequest();

			}

		}











	}
}
