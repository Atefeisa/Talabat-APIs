using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository.Specifications
{
	
	public class SpecifcationsEvaluator<TEntity> where TEntity : BaseEntity
	{
		public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> spec)
		{
			
			var Query = inputQuery;     // _context.Set<Product>()


			if (spec.Criteria is not null)
				 Query = Query.Where(spec.Criteria);
				                     // _context.Set<Porduct>().Where(P => P.Id == 10)
			


			if (spec.OrderBy is not null)
				 Query = Query.OrderBy(spec.OrderBy);
			                    // _context.Set<Porduct>().OrderBy(P => P.Name)



			if (spec.OrderByDesc is not null)
				 Query = Query.OrderByDescending(spec.OrderByDesc);
			                           // _context.Set<Porduct>().OrderByDescending(P => P.Name)





			if (spec.IsPaginationEnabled)
				 Query = Query.Skip(spec.Skip).Take(spec.Take);	




			//atef ahmed omar mohamed

			Query = spec.Includes.Aggregate(Query, (currentQuery, IncludeExpression)  =>
			                                        currentQuery.Include(IncludeExpression));


			return Query;


			//_context.products.Where(P=>P.Id == id).OrderBy(P => P.Name).Skip(15).Take(5)
			//                 .Include(P => P.Brand).Include(P => P.Category).ToListAsync();


		}
	}



	//_context.products.Include(P => P.Brand).Include(P => P.Category).ToListAsync();
	// _context.products.Where(P => P.Id == id).Include(P => P.Brand).Include(P => P.Category).FirstOrDefaultAsync() as T;



}
