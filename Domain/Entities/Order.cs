using Domain.Interfaces;
using Domain.Types;

namespace Domain.Entities;

public sealed class Order : IBaseEntity, ISoftDeletable
{
	public Guid Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime UpdatedAt { get; set; }
	public bool IsDeleted { get; set; }
	public DateTime? DeletedOn { get; set; }
	public string UserName { get; set; } = null!;
	public DateTime OrderDate { get; set; }
	public double TotalAmount { get; set; }
	public OrderStatus Status { get; set; }

	public User User { get; set; } = null!;
	public OrderDetail? OrderDetail { get; set; }
}