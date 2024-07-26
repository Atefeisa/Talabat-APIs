using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
	public class BasketItemDto
	{
		[Required]
		public int Id { get; set; }

		[Required]
		//[Range(0, int.MaxValue, ErrorMessage = "Quantity must be one item at least")]
		public string productName { get; set; }

		[Required]
		public string PictureUrl { get; set; }

		[Required]
		public string Brand { get; set; }

		[Required]
		public string Type { get; set; }

		[Required]
		//[Range(0.1, double.MaxValue, ErrorMessage = "Price can't be zero")]
		public decimal Price { get; set; }

		[Required]
		public int Quantity { get; set; }
	}
}