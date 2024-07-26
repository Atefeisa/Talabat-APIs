using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Cofigurations
{
	public class EmployeeCofiguration : IEntityTypeConfiguration<Employee>
	{
		public void Configure(EntityTypeBuilder<Employee> builder)
		{

			builder.Property(E => E.Id)
				   .IsRequired();



			builder.Property(E => E.Name)
				   .IsRequired()
				   .HasMaxLength(100);


			builder.Property(E => E.Salary)
				   .HasColumnType("decimal(18,2)");




		}
	}


}
