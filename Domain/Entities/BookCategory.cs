using Domain.Interfaces;

namespace Domain.Entities;

public sealed class BookCategory : IBaseEntity, ISoftDeletable
{
	public Guid Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }
	public bool IsDeleted { get; set; }
	public DateTime? DeletedOn { get; set; }

	public Guid BookId { get; set; }
	public Guid CategoryId { get; set; }

	public Book Book { get; set; } = null!;
	public Category Category { get; set; } = null!;
}