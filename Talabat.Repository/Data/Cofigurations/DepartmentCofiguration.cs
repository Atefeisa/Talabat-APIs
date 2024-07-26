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
	public class DepartmentCofiguration : IEntityTypeConfiguration<Department>
	{
		public void Configure(EntityTypeBuilder<Department> builder)
		{

			builder.Property(D => D.Id)
				   .IsRequired()
				   .UseIdentityColumn(10, 10);


			builder.Property(D => D.Name)
				   .IsRequired()
				   .HasMaxLength(100);

		}


	}
}
