﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Order;

namespace Talabat.Repository.Data.Configurations
{
    public class OrderItemConfigurations : IEntityTypeConfiguration< OrderItem >
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {

            builder.Property(OI => OI.Price)
                   .HasColumnType("decimal(18,2)");

            builder.OwnsOne( OI => OI.Product, P => P.WithOwner() );


		;

		}
	}
}
