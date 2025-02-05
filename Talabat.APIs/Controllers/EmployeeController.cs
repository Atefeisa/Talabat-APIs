﻿using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Specifications.EmployeeSpecs;

namespace Talabat.APIs.Controllers
{
	public class EmployeeController : ApiBaseController
	{

		private readonly IGenericRepository<Employee> _employeeRepo;

		public EmployeeController(IGenericRepository<Employee> employeeRepo)
		{
			_employeeRepo = employeeRepo;
		}



		[HttpGet]

		public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
		{
			var spec = new EmployeeWithDepartmentSpecifications();

			var employees = await _employeeRepo.GetAllWithSpecAsync(spec);

			return Ok(employees);

		}





		[ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
		[HttpGet(template: "{id}")]
		public async Task<ActionResult<Employee>> GetEmployeesId(int id)
		{


			var specs = new EmployeeWithDepartmentSpecifications(id);

			var employee = await _employeeRepo.GetEntityWithSpecAsync(specs);


			if (employee is null)
				return NotFound( new ApiResponse(404) );
			

			return Ok(employee);


		}








	}
}
