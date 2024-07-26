using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Windows.Markup;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Specifications.ProductSpecs;
using Talabat.Core.UnitOfWork.Interfaces;

namespace Talabat.APIs.Controllers
{
  
    public class ProductsController : ApiBaseController

    {
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
 
		public ProductsController(IMapper mapper,IUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;	
		}


		#region    1  -  Get Products



		[Authorize]
		[HttpGet]  // GET : /api/products
		public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts( [FromQuery] ProductSpecParams productSpec)
		{



			var spec = new ProductWithBrandAndTypeSpecifications(productSpec);

			var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);



			var MappdProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

			var countSpec = new ProductWithFilterationForCountSpecifications(productSpec);

			var count = await _unitOfWork.Repository<Product>().GetCountAsync(countSpec);

			return Ok(new Pagination<ProductToReturnDto>( productSpec.PageIndex, productSpec.PageSize, count, MappdProducts ));


		}



		#endregion




		#region    2   -  Get Products By Id




		[ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet(template: "{id}")]    // GET : /api/products/3
		public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
		{

			// var product = await _productRepo.GetAsync(id);


			var specs = new ProductWithBrandAndTypeSpecifications(id);

			var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(specs);


			if (product is null)
				return NotFound(new ApiResponse(400));


			var result = _mapper.Map<Product, ProductToReturnDto>(product);


			return Ok(result);   //200


		}



		#endregion




		#region    3   -  Get Brands



		[Authorize]
		[HttpGet("Brands")]  // GET : /api/products/Brands
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

			return Ok(brands);

		}



		#endregion




		#region    3   -  Get Types




		[Authorize]
		[HttpGet("types")]  // GET : /api/products/types
		public async Task<ActionResult<IReadOnlyList<ProductType>>> GetCategories()
		{
			var types = await _unitOfWork.Repository<ProductType>().GetAllAsync();

			return Ok(types);

		}



		#endregion








	}
}
