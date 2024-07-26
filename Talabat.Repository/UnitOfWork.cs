using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.UnitOfWork.Interfaces;
using Talabat.Repository.Data;
using Talabat.Repository.Repositories;

namespace Talabat.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly StoreDbContext _context;
		private Hashtable _repositories;

		public UnitOfWork(StoreDbContext context)
        {
			_context = context;
			_repositories = new Hashtable();
		}

		

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();


	     public ValueTask DisposeAsync() => _context.DisposeAsync();



		public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
		{

			var Type = typeof(TEntity).Name;


			if (! _repositories.ContainsKey(Type))
			{
				var Repository = new GenericRepository<TEntity>(_context);

				_repositories.Add(Type, Repository);
			}

			return _repositories[Type] as IGenericRepository<TEntity>;
		}



	}
}
