﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Interfaces;

namespace Talabat.Core.UnitOfWork.Interfaces
{
	public interface IUnitOfWork : IAsyncDisposable
	{

		// Create Repo Of Any Entity
    	IGenericRepository<TEntity>Repository<TEntity>() where TEntity : BaseEntity;

		Task<int> CompleteAsync();

	}


}
