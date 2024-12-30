using Domain.Entities;
using Domain.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
	public void Configure(EntityTypeBuilder<Order> builder)
	{
		builder.Property(e => e.OrderDate)
			.HasColumnType("timestamp")
			.HasDefaultValueSql("CURRENT_TIMESTAMP")
			.IsRequired();

		builder.Property(e => e.UserName)
			.HasMaxLength(256)
			.IsRequired();

		builder.Property(e => e.Status)
			.HasDefaultValue(OrderStatus.New);
	}
}