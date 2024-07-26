using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecs
{
	public class ProductWithBrandAndTypeSpecifications : BaseSpecifications<Product>
	{

		//This Constructor Will Be Used For Creating Object To Get All Products
		public ProductWithBrandAndTypeSpecifications( ProductSpecParams productSpec )
			: base (  P =>

			(string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search) ) &&

			(! productSpec.BrandId.HasValue || P.ProductBrandId == productSpec.BrandId )  &&

			(! productSpec.TypeId.HasValue || P.ProductTypeId == productSpec.TypeId )  
			
			
			)       

		{

			Includes.Add(P => P.ProductBrand);
			Includes.Add(P => P.ProductType);


			if (!string.IsNullOrEmpty(productSpec.Sort))
			{
				switch (productSpec.Sort)
				{

					case "PriceAsc":
						//orderBy = P =>P.Price
						AddOrderBy(P => P.Price);
					break;


					case "PriceDesc":
						//orderByDesc = P =>P.Price
						AddOrderByDesc(P => P.Price);
						break;


					default:

						AddOrderBy(P => P.Name);

					break;


				} 
			}
			else
			{
				AddOrderBy(P => P.Name);
			}




			//Total = 18
			//PageSiza = 5*4
			//pageIndex = 5



			ApplyPagination( productSpec.PageSize  * ( productSpec.PageIndex - 1 ) , productSpec.PageSize );



                
        }




		public ProductWithBrandAndTypeSpecifications( int id )	: base(  P => P.Id == id  )            // ( Includes and Criteria )
		{

			Includes.Add(P => P.ProductBrand);

			Includes.Add(P => P.ProductType);

		}





	}
}
