﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity 
    {

        //GetAll
        //GetById


        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);


		// with specifications
		Task<T?> GetEntityWithSpecAsync(ISpecifications<T> spec);

		Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);

        Task<int> GetCountAsync ( ISpecifications<T>  spec );


        Task AddAsync( T item );

        void Delete( T item );

        void Update( T item );

	}
}
