using Domain.Interfaces;

namespace Domain.Entities;

public sealed class Book : IBaseEntity, ISoftDeletable
{
	public Guid Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }

	public string Title { get; set; } = null!;
	public bool IsDeleted { get; set; }
	public DateTime? DeletedOn { get; set; }

	public string Author { get; set; } = null!;
	public string Publisher { get; set; } = null!;

	public DateTime PublicationDate { get; set; }

	public string Isbn10 { get; set; } = null!;
	public string Isbn13 { get; set; } = null!;

	public string Description { get; set; } = null!;
	public int Quantity { get; set; }
	public double Price { get; set; }

	public IEnumerable<OrderDetail> OrderDetails { get; } = [];
	public IEnumerable<BookCategory> BookCategories { get; } = [];
}