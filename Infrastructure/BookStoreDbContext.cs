using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Configuration;
using Infrastructure.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Infrastructure;

public class BookStoreDbContext(
	DbContextOptions<BookStoreDbContext> options,
	AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor,
	SoftDeleteInterceptor softDeleteInterceptor)
	: IdentityDbContext<User>(options)
{
	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		foreach (var entityType in builder.Model.GetEntityTypes()
				.Where(e => typeof(IBaseEntity).IsAssignableFrom(e.ClrType)))
		{
			builder.Entity(entityType.ClrType)
				.Property(nameof(IBaseEntity.CreatedAt))
				.HasColumnType("timestamp with time zone")
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.IsRequired();

			builder.Entity(entityType.ClrType)
				.Property(nameof(IBaseEntity.UpdatedAt))
				.HasColumnType("timestamp with time zone")
				.IsRequired();
		}

		foreach (var entityType in builder.Model.GetEntityTypes()
					 .Where(e => typeof(ISoftDeletable).IsAssignableFrom(e.ClrType)))
		{
			builder.Entity(entityType.ClrType)
				.Property(nameof(ISoftDeletable.DeletedOn))
				.HasColumnType("timestamp with time zone")
				.HasDefaultValueSql("CURRENT_TIMESTAMP")
				.IsRequired();
		}

		ModelBuilderConfiguration.Apply(builder);

		builder.Entity<Book>().HasQueryFilter(r => !r.IsDeleted);
		builder.Entity<Category>().HasQueryFilter(r => !r.IsDeleted);
		builder.Entity<BookCategory>().HasQueryFilter(r => !r.IsDeleted);
		builder.Entity<Order>().HasQueryFilter(r => !r.IsDeleted);
		builder.Entity<OrderDetail>().HasQueryFilter(r => !r.IsDeleted);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
		optionsBuilder.AddInterceptors(auditableEntitySaveChangesInterceptor, softDeleteInterceptor);
	}

	public DbSet<Book> Books { get; init; } = default!;
	public DbSet<BookCategory> BookCategories { get; init; } = default!;
	public DbSet<Category> Categories { get; init; } = default!;
	public DbSet<Order> Orders { get; init; } = default!;
	public DbSet<OrderDetail> OrderDetails { get; init; } = default!;
}