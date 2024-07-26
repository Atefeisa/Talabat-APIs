﻿using Talabat.Core.Entities;

namespace Talabat.APIs.Dtos
{
	public class CustomerBasketDto
	{

		public string Id { get; set; }

		public List<BasketItemDto> Items { get; set; }


		public string? PaymentIntentId { get; set; }
		public string? CLientSecret { get; set; }
		public int? DeliveryMethodId { get; set; }

	}
}
