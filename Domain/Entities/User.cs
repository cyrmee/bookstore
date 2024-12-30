using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public sealed class User : IdentityUser
{
	public IEnumerable<Order> Orders { get; } = [];
}