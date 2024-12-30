using Domain.Interfaces;

namespace Domain.Entities;

public sealed class OrderDetail : IBaseEntity, ISoftDeletable
{
	public Guid Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }
	public bool IsDeleted { get; set; }
	public DateTime? DeletedOn { get; set; }

	public Guid OrderId { get; set; }
	public Guid BookId { get; set; }
	public int Quantity { get; set; }
	public double Price { get; set; }

	public Book Book { get; set; } = null!;
	public Order Order { get; set; } = null!;
}