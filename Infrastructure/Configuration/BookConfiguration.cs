using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Configuration;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
	public void Configure(EntityTypeBuilder<Book> builder)
	{
		builder.Property(e => e.PublicationDate)
			.HasColumnType("timestamp with time zone");

		builder.Property(b => b.Title)
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(b => b.Author)
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(b => b.Publisher)
			.HasMaxLength(200)
			.IsRequired();

		builder.Property(b => b.Isbn10)
			.HasMaxLength(20);

		builder.Property(b => b.Isbn13)
			.HasMaxLength(20);

		builder.Property(b => b.Description)
			.HasMaxLength(200);

		builder.HasMany(e => e.OrderDetails)
			.WithOne(e => e.Book)
			.HasForeignKey(e => e.BookId)
			.IsRequired();

		builder.HasMany(e => e.BookCategories)
			.WithOne(e => e.Book)
			.HasForeignKey(e => e.BookId)
			.IsRequired();
	}
}