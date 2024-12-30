using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
	public void Configure(EntityTypeBuilder<Category> builder)
	{
		builder.Property(e => e.Name)
			.HasMaxLength(200)
			.IsRequired();

		builder.HasMany(e => e.BookCategories)
			.WithOne(e => e.Category)
			.HasForeignKey(e => e.CategoryId)
			.IsRequired();
	}
}