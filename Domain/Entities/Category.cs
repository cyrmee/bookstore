using Domain.Interfaces;

namespace Domain.Entities;

public sealed class Category : IBaseEntity, ISoftDeletable
{
	public Guid Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }
	public bool IsDeleted { get; set; }
	public DateTime? DeletedOn { get; set; }

	public string Name { get; set; } = null!;

	public IEnumerable<BookCategory> BookCategories { get; } = [];
}