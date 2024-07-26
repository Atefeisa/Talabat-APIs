using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Interfaces;

namespace Talabat.Repository.Repositories
{
	public class BasketRepository : IBasketRepository
	{
		 private readonly IDatabase _database;
        public BasketRepository( IConnectionMultiplexer redis)
        {
			_database = redis.GetDatabase();
        }




		public async Task<bool> DeletBasketAsync(string basketId)
		{

			return await _database.KeyDeleteAsync(basketId);

		}




		public async Task<CustomerBasket?> GetBasketAsync(string basketId)
		{

			var basket = await _database.StringGetAsync(basketId);


			// transfer from json (redis value) to CustomerBasket  ==> deserialize

			return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);


		}




		public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)   //create or update
		{
			// transfer from CustomerBasket to json (redis value) as it didn't accept customerbasket it accept redis key and value ==> serialize

			var JsonBasket = JsonSerializer.Serialize(basket) ;

			var createdOrUpdated = await _database.StringSetAsync(basket.Id,JsonBasket,TimeSpan.FromDays(30));

			if (createdOrUpdated is false)
				        return null;

			return await GetBasketAsync(basket.Id);

		}


	}
}
