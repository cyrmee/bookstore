using Domain.Interfaces;

namespace Domain.Entities;

public sealed class BookCategory : IBaseEntity
{
	public Guid Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }

	public Guid BookId { get; set; }
	public Guid CategoryId { get; set; }

	public Book Book { get; set; } = null!;
	public Category Category { get; set; } = null!;
}