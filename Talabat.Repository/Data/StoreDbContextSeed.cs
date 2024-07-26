using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order;

namespace Talabat.Repository.Data
{
    public static class StoreDbContextSeed
    {

        public static async Task SeedAsync(StoreDbContext _context)
        {


             




			// 1 - Product Brands


			if (_context.ProductBrands.Count() == 0)
            {


                // 1. Read Data From Json File

                var brandtData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");


                // 2. Convert Json String To The Needed Type

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandtData);

                if (brands?.Count() > 0)
                {
                    foreach (var brand in brands)
                    {
                        _context.ProductBrands.Add(brand);
                    }

                    await _context.SaveChangesAsync();
                }


            }


            // ================================    
            // ================================
            // ================================


            // 2 - Product Types 


            if (_context.ProductTypes.Count() == 0)
            {



                // 1- Read Data From Json File

                var typeData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");

                // 2 - Convert Json String To The Needed Type

                var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);

                if (types?.Count() > 0)
                {
                    foreach (var type in types)
                    {

                        _context.ProductTypes.Add(type);

                        await _context.SaveChangesAsync();

                    }




                }

            }






			// ================================    
			// ================================
			// ================================




			// 3 - Products 


			if (_context.Products.Count() == 0)
			{


				// 1- Read Data From Json File

				var productData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");


				// 2 - Convert Json String To The Needed Type

				var products = JsonSerializer.Deserialize<List<Product>>(productData);

				if (products?.Count() > 0)
				{
					foreach (var product in products)
					{
						//_context.Set<Product>().Add(product);

						_context.Products.Add(product);

						await _context.SaveChangesAsync();

					}




				}


			}




			


			// ================================    
			// ================================
			// ================================


			// 4 - Delivery


			if (_context.DeliveryMethods.Count() == 0)
            {


                // 1- Read Data From Json File

                var DeliveryData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");


                // 2 - Convert Json String To The Needed Type

                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);

                if (deliveryMethods?.Count() > 0)
                {
                    foreach (var deliveryMethod in deliveryMethods)
                    {
                        _context.DeliveryMethods.Add(deliveryMethod);


                        await _context.SaveChangesAsync();

                    }


                }


            }
















        }










    }
}
