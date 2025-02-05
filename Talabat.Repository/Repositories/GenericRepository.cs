﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;
using Talabat.Repository.Specifications;

namespace Talabat.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {

        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }
       
        
        
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {

            if (typeof(T) == typeof(Product))
            {
            
                // return (IEnumerable<T>)await _context.Products.Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync();

                return (IReadOnlyList<T>)await _context.Products.OrderByDescending(P => P.Price ).Skip(400).Take(50)
                                                                .Include(P => P.ProductBrand).Include(P => P.ProductType).ToListAsync();

			}
			return await _context.Set<T>().ToListAsync();
        }



		public async Task<T?> GetByIdAsync(int id)
		{
			if (typeof(T) == typeof(Product))
			{
				return await _context.Products.Where(P => P.Id == id).Include(P => P.ProductBrand).Include(P => P.ProductType).FirstOrDefaultAsync() as T;
			}

			return await _context.Set<T>().FindAsync(id);
		}




        public IQueryable<T> ApplySpecifications( ISpecifications<T> spec)
        {
            return SpecifcationsEvaluator<T>.GetQuery(_context.Set<T>(), spec);
        }


		public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
		{

			return await ApplySpecifications(spec).ToListAsync();

		}


		public async Task<T?> GetEntityWithSpecAsync(ISpecifications<T> spec)
		{

			return await ApplySpecifications(spec).FirstOrDefaultAsync();

		}




		public async Task<int> GetCountAsync(ISpecifications<T> spec)
		{
			return await ApplySpecifications(spec).CountAsync();
		}





		public async Task AddAsync(T item) => await _context.Set<T>().AddAsync(item);


		public void Delete(T item) => _context.Set<T>().Remove(item);


		public void Update(T item) => _context.Set<T>().Update(item);





		//public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
		//{
		//	return await SpecifcationsEvaluator<T>.GetQuery(_context.Set<T>(), spec).ToListAsync();
		//}



		//public Task<T?> GetByIdWithSpecAsync(ISpecifications<T> spec)
		//{
		//	return  SpecifcationsEvaluator<T>.GetQuery(_context.Set<T>(), spec).FirstOrDefaultAsync();
		//}



		//public async Task<int> GetCountAsync(ISpecifications<T> spec)
		//{
		//    return await SpecifcationsEvaluator<T>.GetQuery( _context.Set<T>() ,spec).CountAsync();
		//}




	}
}
